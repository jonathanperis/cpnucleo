-- Rewrites the Bogus-generated demo dataset into a coherent project workspace:
-- industry companies, their products and initiatives, a delivery board (Backlog .. Done),
-- feature/bug/chore tasks with matching time entries and blocker notes, readable logins,
-- and task schedules whose board column follows their dates.
--
-- Only rows that still carry generated text are touched: names made of Bogus Hacker words
-- ("monitor transmitting back-end"), Hacker.Phrase descriptions, blank legacy descriptions and
-- "learner-NNNNNN" logins. Rows created by people, the demo account, Ids, relations, Active,
-- DeletedAt, CreatedAt and UpdatedAt are never changed, so running it again changes nothing.
--
-- One statement, so it is atomic with or without an explicit transaction. It runs at the end of
-- FakeDataCsvImporter, as the legacy compose.yaml init script, and by hand on an existing
-- database through scripts/apply-demo-workspace-names.sh. Keep docker-entrypoint-initdb.d/
-- 004-demo-workspace-names.sql identical to this file.
DO $demo_workspace_names$
DECLARE
    hacker_name constant text := '^(alarm|application|array|bandwidth|bus|capacitor|card|circuit|driver|feed|firewall|hard drive|interface|matrix|microchip|monitor|panel|pixel|port|program|protocol|sensor|system|transmitter) (backing up|bypassing|calculating|compressing|connecting|copying|generating|hacking|indexing|navigating|overriding|parsing|programming|quantifying|synthesizing|transmitting) (1080p|auxiliary|back-end|bluetooth|cross-platform|digital|haptic|mobile|multi-byte|neural|online|open-source|optical|primary|redundant|solid state|virtual|wireless)$';
    hacker_phrase constant text := '^(If we |We need to |Try to |You can''t |Use the |The |I''ll |[a-z]+ing (up )?the ).*!$';
    stem_count constant int := 60;
    industry_count constant int := 14;
    systems_per_industry constant int := 12;
    initiative_count constant int := 32;
    verbs_per_role constant int := 12;
    features_per_project constant int := 40;
    workflow_names constant text[] := ARRAY['Backlog', 'To Do', 'In Progress', 'In Review', 'Testing', 'Done'];
    type_names constant text[] := ARRAY['Feature', 'Bug', 'Chore', 'Spike', 'Support', 'Research'];
    workflow_ids uuid[];
    overflow bigint;
    changed bigint;
    summary text := '';
BEGIN
    IF to_regclass('public."Assignments"') IS NULL OR to_regclass('public."Users"') IS NULL THEN
        RAISE NOTICE 'Demo workspace names skipped: the Cpnucleo schema does not exist.';
        RETURN;
    END IF;

    -- Catalogs: (kind, group, index, value, detail).
    CREATE TEMP TABLE demo_names_catalog (kind text, grp int, idx int, val text, detail text);

    INSERT INTO demo_names_catalog (kind, grp, idx, val)
    SELECT 'stem', 0, ordinality - 1, val FROM unnest(ARRAY[
        'Northwind', 'Blue Harbor', 'Cedar Ridge', 'Atlas', 'Lumen', 'Brightwave', 'Ironbridge', 'Silver Pine', 'Redwood', 'Clearpath',
        'Summit', 'Harborview', 'Oakline', 'Polaris', 'Riverstone', 'Greenfield', 'Skyline', 'Bluefin', 'Copperleaf', 'Evergreen',
        'Maple Street', 'Highland', 'Starling', 'Westgate', 'Granite Peak', 'Sunfield', 'Driftwood', 'Pinecrest', 'Meridian', 'Lakeshore',
        'Falcon', 'Horizon', 'Keystone', 'Larkspur', 'Moonstone', 'Nimbus', 'Orchard', 'Pioneer', 'Quarry Lane', 'Rosewood',
        'Sterling', 'Tidewater', 'Unity', 'Vantage', 'Willow Creek', 'Zenith', 'Amberly', 'Bayside', 'Crescent', 'Dovetail',
        'Elmstead', 'Foxglove', 'Goldcrest', 'Hollowbrook', 'Indigo', 'Juniper', 'Kestrel', 'Lighthouse', 'Mosaic', 'Northstar'
    ]) WITH ORDINALITY AS u(val);

    INSERT INTO demo_names_catalog (kind, grp, idx, val)
    SELECT 'city', 0, ordinality - 1, val FROM unnest(ARRAY[
        'Lisbon', 'Porto', 'São Paulo', 'Curitiba', 'Austin', 'Denver', 'Toronto', 'Dublin',
        'Berlin', 'Amsterdam', 'Madrid', 'Lyon', 'Kraków', 'Cape Town', 'Singapore', 'Sydney'
    ]) WITH ORDINALITY AS u(val);

    INSERT INTO demo_names_catalog (kind, grp, idx, val, detail) VALUES
        ('industry', 0, 0, 'Logistics', 'Freight and last-mile delivery operator'),
        ('industry', 0, 1, 'Health', 'Network of outpatient clinics'),
        ('industry', 0, 2, 'Bank', 'Regional retail bank'),
        ('industry', 0, 3, 'Retail', 'Omnichannel retail chain'),
        ('industry', 0, 4, 'Labs', 'Product design and software studio'),
        ('industry', 0, 5, 'Energy', 'Renewable energy provider'),
        ('industry', 0, 6, 'Insurance', 'Home and auto insurer'),
        ('industry', 0, 7, 'Media', 'Digital publishing group'),
        ('industry', 0, 8, 'Education', 'Online learning platform'),
        ('industry', 0, 9, 'Manufacturing', 'Industrial parts manufacturer'),
        ('industry', 0, 10, 'Travel', 'Travel booking agency'),
        ('industry', 0, 11, 'Foods', 'Food distribution company'),
        ('industry', 0, 12, 'Telecom', 'Broadband and mobile carrier'),
        ('industry', 0, 13, 'Software', 'B2B SaaS company');

    -- Products per industry (disjoint, so project names are unique across industries).
    INSERT INTO demo_names_catalog (kind, grp, idx, val)
    SELECT 'system', s.grp, u.ordinality - 1, u.val
    FROM (VALUES
        (0, ARRAY['Shipment Tracking', 'Route Planner', 'Warehouse Scanner App', 'Fleet Dashboard', 'Carrier Portal', 'Dock Scheduling', 'Freight Quotes', 'Customs Filing', 'Proof of Delivery', 'Returns Hub', 'Load Optimizer', 'Driver App']),
        (1, ARRAY['Patient Records', 'Appointment Scheduling', 'Telehealth', 'Lab Results', 'Prescription Refills', 'Patient Portal', 'Clinic Billing', 'Care Plans', 'Nurse Rostering', 'Insurance Verification', 'Vaccination Registry', 'Referral Management']),
        (2, ARRAY['Mobile Banking', 'Loan Origination', 'Card Management', 'Fraud Monitoring', 'Open Banking API', 'Branch Booking', 'KYC Onboarding', 'Statement Archive', 'Savings Goals', 'Wire Transfers', 'Credit Scoring', 'ATM Locator']),
        (3, ARRAY['Online Store', 'Checkout', 'Loyalty Program', 'Store Inventory', 'Click and Collect', 'Product Catalog', 'Gift Cards', 'Price Labels', 'Returns Portal', 'Customer Reviews', 'Promotions Engine', 'Point of Sale']),
        (4, ARRAY['Design System', 'Prototype Studio', 'Client Portal', 'Time Tracking', 'Asset Library', 'Feedback Board', 'Proposal Builder', 'Usability Lab', 'Component Docs', 'Sprint Reports', 'Brand Guidelines', 'Mockup Reviews']),
        (5, ARRAY['Smart Meter Portal', 'Outage Map', 'Solar Monitoring', 'Field Crew App', 'Grid Analytics', 'Tariff Calculator', 'EV Charging', 'Meter Reading', 'Energy Insights', 'Asset Maintenance', 'Demand Response', 'Billing Statements']),
        (6, ARRAY['Claims Processing', 'Policy Admin', 'Quote Engine', 'Agent Portal', 'Underwriting Rules', 'Renewals', 'Broker API', 'Customer Self-Service', 'Fraud Screening', 'Document Vault', 'Premium Billing', 'Roadside Assistance']),
        (7, ARRAY['Publishing CMS', 'Paywall', 'Newsletter', 'Video Player', 'Podcast Feed', 'Ad Manager', 'Comments', 'Editorial Calendar', 'Photo Archive', 'Live Blog', 'Recommendations', 'Audience Analytics']),
        (8, ARRAY['Course Catalog', 'Learning Portal', 'Gradebook', 'Enrollment', 'Virtual Classroom', 'Quiz Engine', 'Certificates', 'Tutor Matching', 'Student Records', 'Assignment Submissions', 'Discussion Forums', 'Attendance Tracker']),
        (9, ARRAY['Production Planning', 'Quality Control', 'Machine Monitoring', 'Supplier Portal', 'Bill of Materials', 'Maintenance Scheduler', 'Shop Floor App', 'Inventory Control', 'Safety Reporting', 'Batch Tracking', 'Shift Handover', 'Purchase Orders']),
        (10, ARRAY['Booking Engine', 'Itinerary App', 'Hotel Search', 'Flight Status', 'Loyalty Miles', 'Travel Insurance', 'Car Rental', 'Group Bookings', 'Visa Assistant', 'Trip Reviews', 'Fare Alerts', 'Concierge Chat']),
        (11, ARRAY['Order Management', 'Delivery Tracker', 'Menu Builder', 'Supplier Orders', 'Cold Chain Monitor', 'Recipe Costing', 'Wholesale Portal', 'Route Dispatch', 'Allergen Labels', 'Stock Forecast', 'Restaurant App', 'Invoice Matching']),
        (12, ARRAY['Customer Portal', 'Plan Selector', 'Network Status', 'SIM Activation', 'Billing Platform', 'Number Porting', 'Field Installation', 'Roaming', 'Usage Dashboard', 'Support Desk', 'Device Shop', 'Fiber Coverage Map']),
        (13, ARRAY['Admin Console', 'Subscription Billing', 'Public API', 'Developer Portal', 'Onboarding Flow', 'Usage Analytics', 'Single Sign-On', 'Audit Logging', 'Feature Flags', 'Status Page', 'Help Center', 'Integrations Hub'])
    ) AS s(grp, vals)
    CROSS JOIN LATERAL unnest(s.vals) WITH ORDINALITY AS u(val);

    INSERT INTO demo_names_catalog (kind, grp, idx, val)
    SELECT 'initiative', 0, ordinality - 1, val FROM unnest(ARRAY[
        'Redesign', 'Cloud Migration', 'v2 Launch', 'Modernization', 'Performance Overhaul', 'Accessibility Audit', 'MVP', 'Localization',
        'Security Hardening', 'Integration', 'Rollout', 'Refactor', 'Mobile Support', 'Self-Service', 'Automation', 'Data Cleanup',
        'Observability', 'Cost Reduction', 'Pilot', 'Replatform', 'Compliance Update', 'Onboarding Revamp', 'API Upgrade', 'Beta',
        'Offline Mode', 'Reliability Sprint', 'Analytics', 'Consolidation', 'Multi-Region', 'UX Polish', 'Go-Live', 'Phase 2'
    ]) WITH ORDINALITY AS u(val);

    -- Task subjects: 24 per industry (indexes 0-23) plus 16 shared ones (indexes 24-39).
    INSERT INTO demo_names_catalog (kind, grp, idx, val)
    SELECT 'feature', s.grp, u.ordinality - 1, u.val
    FROM (VALUES
        (0, ARRAY['shipment labels', 'delivery windows', 'route optimization', 'barcode scanning', 'proof-of-delivery photos', 'carrier rates', 'pallet tracking', 'customs forms', 'dock appointments', 'driver check-in', 'ETA notifications', 'returns intake', 'fuel reports', 'load manifests', 'geofence alerts', 'damage claims',
             'cold storage alerts', 'delivery signatures', 'shipment splitting', 'warehouse slots', 'pickup requests', 'carrier scorecards', 'cross-dock transfers', 'delivery exceptions']),
        (1, ARRAY['appointment reminders', 'lab result sharing', 'consent forms', 'prescription renewals', 'visit summaries', 'insurance cards', 'triage questionnaire', 'provider calendar', 'patient messaging', 'allergy list', 'discharge notes', 'video visits', 'copay estimates', 'care team view', 'waitlist', 'referral letters',
             'patient intake', 'vital signs chart', 'immunization records', 'medication list', 'clinic hours', 'telehealth waiting room', 'lab orders', 'visit invoices']),
        (2, ARRAY['card freeze', 'transfer limits', 'loan calculator', 'statement download', 'KYC document upload', 'savings goals', 'recurring transfers', 'spending insights', 'PIN reset', 'overdraft alerts', 'beneficiary list', 'cheque deposit', 'credit limit requests', 'travel notices', 'account nicknames', 'interest summary',
             'direct debits', 'foreign exchange rates', 'joint accounts', 'mortgage applications', 'card disputes', 'standing orders', 'tax certificates', 'push payment alerts']),
        (3, ARRAY['coupon codes', 'wish lists', 'product reviews', 'size guide', 'gift card balance', 'store pickup', 'stock alerts', 'abandoned cart emails', 'price matching', 'bundle offers', 'loyalty points', 'receipt lookup', 'product comparison', 'back-in-stock alerts', 'shelf labels', 'order tracking',
             'gift wrapping', 'delivery slots', 'store locator', 'product filters', 'flash sales', 'return labels', 'pickup lockers', 'price drop alerts']),
        (4, ARRAY['design tokens', 'component previews', 'client approvals', 'feedback pins', 'asset tagging', 'version history', 'proposal templates', 'usability notes', 'color contrast checks', 'icon set', 'handoff specs', 'sprint summary', 'Figma import', 'review comments', 'style guide pages', 'prototype links',
             'moodboards', 'client invoices', 'project timelines', 'retainer hours', 'design QA checklist', 'user interview notes', 'animation specs', 'case study pages']),
        (5, ARRAY['meter readings', 'outage reports', 'solar yield charts', 'tariff comparison', 'EV charger booking', 'usage alerts', 'payment plans', 'crew dispatch', 'asset inspections', 'peak hour pricing', 'carbon savings', 'maintenance tickets', 'moving home flow', 'smart thermostat link', 'energy tips', 'budget billing',
             'direct debit setup', 'meter exchange booking', 'feed-in tariffs', 'battery storage view', 'home energy audit', 'green energy certificates', 'outage compensation', 'tariff switching']),
        (6, ARRAY['claim photos', 'quote comparison', 'policy documents', 'renewal reminders', 'premium installments', 'accident reports', 'coverage summary', 'beneficiary changes', 'underwriting notes', 'broker commissions', 'roadside requests', 'no-claims bonus', 'deductible options', 'adjuster scheduling', 'risk questionnaire', 'proof of insurance',
             'pet cover', 'home inventory', 'claim status tracker', 'policy cancellation', 'multi-car discount', 'windscreen claims', 'travel cover', 'payment reminders']),
        (7, ARRAY['article drafts', 'paywall meter', 'newsletter signup', 'video captions', 'podcast chapters', 'ad slots', 'comment moderation', 'editorial calendar', 'photo credits', 'live updates', 'related articles', 'reading time', 'author pages', 'breaking news alerts', 'subscription offers', 'content tags',
             'push alerts', 'print edition', 'video playlists', 'reader polls', 'corrections log', 'embargo dates', 'syndication feed', 'podcast transcripts']),
        (8, ARRAY['course enrollment', 'quiz timer', 'certificate download', 'grade export', 'lesson bookmarks', 'discussion threads', 'assignment uploads', 'attendance check-in', 'tutor booking', 'progress tracker', 'video lessons', 'rubric scoring', 'class roster', 'deadline reminders', 'plagiarism check', 'learning paths',
             'parent portal', 'exam timetable', 'course reviews', 'scholarship applications', 'lesson notes', 'peer review', 'office hours', 'transcript requests']),
        (9, ARRAY['work orders', 'defect reports', 'machine alerts', 'batch traceability', 'supplier quotes', 'BOM revisions', 'preventive maintenance', 'shift notes', 'safety incidents', 'spare parts', 'scrap tracking', 'line efficiency', 'purchase approvals', 'calibration logs', 'material requests', 'label printing',
             'tool tracking', 'production schedule', 'downtime reasons', 'quality audits', 'inbound inspections', 'packing lists', 'energy usage', 'operator training']),
        (10, ARRAY['seat selection', 'fare alerts', 'itinerary sharing', 'hotel filters', 'flight status updates', 'miles balance', 'booking changes', 'travel insurance add-on', 'car rental extras', 'group invoices', 'visa checklist', 'trip reviews', 'boarding passes', 'baggage add-on', 'cancellation refunds', 'destination guides',
             'price calendar', 'loyalty upgrades', 'check-in reminders', 'multi-city search', 'refund vouchers', 'hotel reviews', 'airport transfers', 'travel documents']),
        (11, ARRAY['menu items', 'delivery slots', 'allergen labels', 'recipe costs', 'supplier orders', 'temperature alerts', 'wholesale pricing', 'standing orders', 'substitutions', 'stock forecast', 'driver routes', 'invoice matching', 'expiry tracking', 'nutrition facts', 'order cut-off times', 'credit notes',
             'pallet returns', 'price lists', 'batch recalls', 'delivery notes', 'customer catalogs', 'minimum order rules', 'supplier certificates', 'waste reports']),
        (12, ARRAY['plan upgrades', 'data usage alerts', 'SIM activation', 'number porting', 'roaming packs', 'bill breakdown', 'outage notices', 'installation booking', 'device financing', 'coverage checker', 'eSIM download', 'family plans', 'top-ups', 'contract renewal', 'speed test', 'router setup',
             'SIM swap', 'international calling', 'parental controls', 'bill disputes', 'loyalty discounts', '5G upgrade', 'home moves', 'voicemail settings']),
        (13, ARRAY['API keys', 'webhooks', 'SSO login', 'audit trail', 'feature flags', 'usage limits', 'team invitations', 'role management', 'billing plans', 'rate limiting', 'status page', 'data export', 'onboarding checklist', 'integration marketplace', 'two-factor login', 'sandbox accounts',
             'SCIM provisioning', 'usage invoices', 'API changelog', 'workspace settings', 'trial extensions', 'IP allowlist', 'export scheduling', 'notification preferences'])
    ) AS s(grp, vals)
    CROSS JOIN LATERAL unnest(s.vals) WITH ORDINALITY AS u(val);

    INSERT INTO demo_names_catalog (kind, grp, idx, val)
    SELECT 'feature', -1, 23 + ordinality, val FROM unnest(ARRAY[
        'CSV export', 'password reset', 'email notifications', 'saved filters', 'bulk edit', 'dark mode', 'activity feed', 'dashboard widgets',
        'profile settings', 'search suggestions', 'file uploads', 'language picker', 'session timeout', 'accessibility labels', 'keyboard shortcuts', 'PDF reports'
    ]) WITH ORDINALITY AS u(val);

    -- Task wording per role: 0 feature, 1 bug (val = title defect, detail = report wording), 2 chore.
    INSERT INTO demo_names_catalog (kind, grp, idx, val)
    SELECT 'verb', 0, ordinality - 1, val FROM unnest(ARRAY[
        'Add', 'Build', 'Implement', 'Design', 'Enable', 'Support', 'Introduce', 'Prototype', 'Ship', 'Extend', 'Improve', 'Redesign'
    ]) WITH ORDINALITY AS u(val);
    INSERT INTO demo_names_catalog (kind, grp, idx, val, detail) VALUES
        ('verb', 1, 0, 'crash', 'crashes'), ('verb', 1, 1, 'wrong totals', 'wrong totals'),
        ('verb', 1, 2, 'timeout', 'timeouts'), ('verb', 1, 3, 'broken layout', 'a broken layout'),
        ('verb', 1, 4, 'duplicate entries', 'duplicate entries'), ('verb', 1, 5, 'missing translations', 'missing translations'),
        ('verb', 1, 6, 'slow loading', 'slow loading'), ('verb', 1, 7, 'validation error', 'validation errors'),
        ('verb', 1, 8, 'broken link', 'broken links'), ('verb', 1, 9, 'race condition', 'intermittent failures'),
        ('verb', 1, 10, 'memory leak', 'growing memory use'), ('verb', 1, 11, 'wrong sorting', 'wrong sorting');
    INSERT INTO demo_names_catalog (kind, grp, idx, val)
    SELECT 'verb', 2, ordinality - 1, val FROM unnest(ARRAY[
        'Upgrade dependencies of', 'Add unit tests for', 'Refactor', 'Document', 'Add monitoring to', 'Clean up logs in',
        'Remove dead code from', 'Add CI checks for', 'Improve error handling in', 'Review permissions of', 'Add metrics to', 'Archive old data from'
    ]) WITH ORDINALITY AS u(val);

    -- Time-entry lines per role; %1$s is the defect, %2$s the subject.
    INSERT INTO demo_names_catalog (kind, grp, idx, val)
    SELECT 'entry', s.grp, u.ordinality - 1, u.val
    FROM (VALUES
        (0, ARRAY['Implemented the main flow for %2$s', 'Wrote unit tests for %2$s', 'Addressed review comments on %2$s', 'Paired on %2$s edge cases',
                  'Updated the API contract for %2$s', 'Demoed %2$s to the product owner', 'Refined the %2$s UI copy', 'Opened the pull request for %2$s']),
        (1, ARRAY['Reproduced the %1$s in %2$s on staging', 'Investigated logs and traces for the %1$s', 'Found the root cause of the %1$s in %2$s', 'Implemented the fix for the %1$s',
                  'Added a regression test for %2$s', 'Verified the %2$s fix in QA', 'Discussed the %1$s impact with support', 'Deployed the %2$s hotfix']),
        (2, ARRAY['Updated %2$s dependencies and ran the test suite', 'Cleaned up %2$s configuration', 'Reviewed CI failures in %2$s', 'Wrote documentation for %2$s',
                  'Removed unused code from %2$s', 'Tuned %2$s dashboards and alerts', 'Reviewed %2$s access rules', 'Planned follow-up work for %2$s'])
    ) AS s(grp, vals)
    CROSS JOIN LATERAL unnest(s.vals) WITH ORDINALITY AS u(val);

    INSERT INTO demo_names_catalog (kind, grp, idx, val)
    SELECT 'blocker', 0, ordinality - 1, val FROM unnest(ARRAY[
        'Waiting on design sign-off for %s', 'Waiting on security review of %s', 'Missing production access for %s', 'Unclear acceptance criteria for %s',
        'Third-party API outage affecting %s', 'Staging environment down for %s', 'Pending legal approval for %s', 'Flaky end-to-end tests in %s',
        'Blocked by upstream release of %s', 'Key reviewer unavailable for %s'
    ]) WITH ORDINALITY AS u(val);
    INSERT INTO demo_names_catalog (kind, grp, idx, val)
    SELECT 'area', 0, ordinality - 1, val FROM unnest(ARRAY[
        'payments', 'authentication', 'reporting', 'search', 'notifications', 'checkout',
        'onboarding', 'data import', 'the mobile release', 'the API gateway', 'billing', 'analytics'
    ]) WITH ORDINALITY AS u(val);
    INSERT INTO demo_names_catalog (kind, grp, idx, val)
    SELECT 'note', 0, ordinality - 1, val FROM unnest(ARRAY[
        '%s. Raised in stand-up; an owner is following up.', '%s. Escalated to the project lead.',
        '%s. Work continues on other tasks meanwhile.', '%s. Revisit at the next planning session.'
    ]) WITH ORDINALITY AS u(val);

    ANALYZE demo_names_catalog;

    -- Capacity guards: refuse to produce duplicates instead of silently repeating names.
    SELECT count(*) INTO overflow FROM "Organizations" WHERE "Name" ~ hacker_name;
    IF overflow > stem_count * industry_count THEN
        RAISE EXCEPTION 'Demo workspace names: % generated organizations exceed the % available names.', overflow, stem_count * industry_count;
    END IF;
    SELECT count(*) INTO overflow FROM "Impediments" WHERE "Name" ~ hacker_name;
    IF overflow > 120 THEN
        RAISE EXCEPTION 'Demo workspace names: % generated impediments exceed the 120 available names.', overflow;
    END IF;
    SELECT count(*) INTO overflow FROM "AssignmentTypes" WHERE "Name" ~ hacker_name;
    IF overflow > cardinality(type_names) THEN
        RAISE EXCEPTION 'Demo workspace names: % generated assignment types exceed the % available names.', overflow, cardinality(type_names);
    END IF;

    -- Organizations: brand stem x industry, a bijection for up to 840 rows.
    CREATE TEMP TABLE demo_names_org (id uuid PRIMARY KEY, industry int, name text, description text, generated boolean);
    INSERT INTO demo_names_org
    SELECT o."Id", (o.n / stem_count + o.n % stem_count) % industry_count,
           s.val || ' ' || i.val, i.detail || ' based in ' || c.val || '.', true
    FROM (SELECT "Id", row_number() OVER (ORDER BY "Id") - 1 AS n FROM "Organizations" WHERE "Name" ~ hacker_name) o
    JOIN demo_names_catalog s ON s.kind = 'stem' AND s.idx = o.n % stem_count
    JOIN demo_names_catalog i ON i.kind = 'industry' AND i.idx = (o.n / stem_count + o.n % stem_count) % industry_count
    JOIN demo_names_catalog c ON c.kind = 'city' AND c.idx = o.n % 16;
    INSERT INTO demo_names_org
    SELECT "Id", abs(hashtext("Id"::text)) % industry_count, "Name", "Description", false
    FROM "Organizations" x WHERE NOT EXISTS (SELECT 1 FROM demo_names_org m WHERE m.id = x."Id");
    ANALYZE demo_names_org;

    UPDATE "Organizations" t SET "Name" = m.name, "Description" = m.description
    FROM demo_names_org m WHERE m.id = t."Id" AND m.generated;
    GET DIAGNOSTICS changed = ROW_COUNT;
    summary := summary || format('Organizations=%s', changed);

    -- Projects: an industry product x initiative, unique within and across industries (the stride 7
    -- is coprime with 32, so each product gets distinct initiatives).
    CREATE TEMP TABLE demo_names_project (id uuid PRIMARY KEY, industry int, name text, generated boolean);
    INSERT INTO demo_names_project
    SELECT p.id, p.industry, s.val || ' ' || i.val, true
    FROM (SELECT x."Id" AS id, o.industry,
                 row_number() OVER (PARTITION BY o.industry ORDER BY x."Id") - 1 AS j
          FROM "Projects" x JOIN demo_names_org o ON o.id = x."OrganizationId"
          WHERE x."Name" ~ hacker_name) p
    JOIN demo_names_catalog s ON s.kind = 'system' AND s.grp = p.industry AND s.idx = p.j % systems_per_industry
    JOIN demo_names_catalog i ON i.kind = 'initiative'
        AND i.idx = ((p.j / systems_per_industry) * 7 + (p.j % systems_per_industry) * 5 + p.industry * 3) % initiative_count;
    SELECT count(*) - (SELECT count(*) FROM demo_names_project) INTO overflow FROM "Projects" WHERE "Name" ~ hacker_name;
    IF overflow > 0 THEN
        RAISE EXCEPTION 'Demo workspace names: % generated projects exceed the % names available per industry.', overflow, systems_per_industry * initiative_count;
    END IF;
    INSERT INTO demo_names_project
    SELECT x."Id", o.industry, x."Name", false
    FROM "Projects" x JOIN demo_names_org o ON o.id = x."OrganizationId"
    WHERE NOT EXISTS (SELECT 1 FROM demo_names_project m WHERE m.id = x."Id");
    ANALYZE demo_names_project;

    UPDATE "Projects" t SET "Name" = m.name FROM demo_names_project m WHERE m.id = t."Id" AND m.generated;
    GET DIAGNOSTICS changed = ROW_COUNT;
    summary := summary || format(', Projects=%s', changed);

    -- Workflows become the board columns in their order (legacy datasets number them from 2062).
    WITH ranked AS (
        SELECT "Id", row_number() OVER (ORDER BY "Order", "Id")::int AS position
        FROM "Workflows" WHERE "Name" ~ hacker_name)
    UPDATE "Workflows" t
    SET "Name" = CASE WHEN ranked.position <= cardinality(workflow_names) THEN workflow_names[ranked.position] ELSE 'Stage ' || ranked.position END
    FROM ranked WHERE ranked."Id" = t."Id";
    GET DIAGNOSTICS changed = ROW_COUNT;
    summary := summary || format(', Workflows=%s', changed);

    -- Assignment types: Feature, Bug, Chore. Each type has a role that picks the task wording.
    CREATE TEMP TABLE demo_names_type (id uuid PRIMARY KEY, role int);
    WITH named AS (
        SELECT "Id", type_names[row_number() OVER (ORDER BY "Id")::int] AS name
        FROM "AssignmentTypes" WHERE "Name" ~ hacker_name)
    UPDATE "AssignmentTypes" t SET "Name" = named.name FROM named WHERE named."Id" = t."Id";
    GET DIAGNOSTICS changed = ROW_COUNT;
    summary := summary || format(', AssignmentTypes=%s', changed);
    INSERT INTO demo_names_type
    SELECT "Id", COALESCE((array_position(type_names, "Name") - 1) % 3, abs(hashtext("Id"::text)) % 3) FROM "AssignmentTypes";
    ANALYZE demo_names_type;

    -- Impediments: blocker x area.
    WITH named AS (
        SELECT x."Id", format(b.val, a.val) AS name
        FROM (SELECT "Id", row_number() OVER (ORDER BY "Id") - 1 AS n FROM "Impediments" WHERE "Name" ~ hacker_name) x
        JOIN demo_names_catalog b ON b.kind = 'blocker' AND b.idx = x.n % 10
        JOIN demo_names_catalog a ON a.kind = 'area' AND a.idx = (x.n / 10 + x.n % 10) % 12)
    UPDATE "Impediments" t SET "Name" = named.name FROM named WHERE named."Id" = t."Id";
    GET DIAGNOSTICS changed = ROW_COUNT;
    summary := summary || format(', Impediments=%s', changed);

    -- Tasks: wording by role; the subject comes from the project's industry or the shared pool.
    -- The stride 7 is coprime with 40, so names are unique within a project and type for up to 480 tasks.
    CREATE TEMP TABLE demo_names_task (
        id uuid PRIMARY KEY, project_id uuid, role int, hours int, has_entries boolean,
        subject text, defect text, name text, description text,
        start_date timestamptz, end_date timestamptz, workflow_order int);
    INSERT INTO demo_names_task (id, project_id, role, hours, has_entries, subject, defect, name, description)
    SELECT a.id, a.project_id, a.role, a.hours, a.has_entries, f.val, v.val,
           CASE a.role WHEN 1 THEN 'Fix ' || v.val || ' in ' || f.val ELSE v.val || ' ' || f.val END,
           CASE a.role
               WHEN 0 THEN format('Deliver %s in %s. Done when it is covered by tests, reviewed and demoed to the product owner.', f.val, p.name)
               WHEN 1 THEN format('Users of %s report %s in %s. Reproduce it on staging, fix the root cause and add a regression test.', p.name, v.detail, f.val)
               ELSE format('Maintenance on %s for %s. No user-facing change expected; keep the build green.', f.val, p.name) END
    FROM (SELECT x."Id" AS id, x."ProjectId" AS project_id, ty.role, x."AmountHours" AS hours,
                 EXISTS (SELECT 1 FROM "Appointments" e WHERE e."AssignmentId" = x."Id") AS has_entries,
                 row_number() OVER (PARTITION BY x."ProjectId", x."AssignmentTypeId" ORDER BY x."Id") - 1 AS k,
                 abs(hashtext(x."ProjectId"::text)) % features_per_project AS shift
          FROM "Assignments" x JOIN demo_names_type ty ON ty.id = x."AssignmentTypeId"
          WHERE x."Name" ~ hacker_name) a
    JOIN demo_names_project p ON p.id = a.project_id
    JOIN demo_names_catalog v ON v.kind = 'verb' AND v.grp = a.role AND v.idx = a.k % verbs_per_role
    JOIN demo_names_catalog f ON f.kind = 'feature'
        AND f.idx = ((a.k / verbs_per_role) * 7 + (a.k % verbs_per_role) * 3 + a.shift) % features_per_project
        AND f.grp = CASE WHEN ((a.k / verbs_per_role) * 7 + (a.k % verbs_per_role) * 3 + a.shift) % features_per_project < 24 THEN p.industry ELSE -1 END;
    SELECT count(*) - (SELECT count(*) FROM demo_names_task) INTO overflow FROM "Assignments" WHERE "Name" ~ hacker_name;
    IF overflow > 0 THEN
        RAISE EXCEPTION 'Demo workspace names: % generated tasks exceed the % names available per project and type.', overflow, verbs_per_role * features_per_project;
    END IF;
    ANALYZE demo_names_task;

    -- Schedules: spread each project's generated tasks from 270 days ago to 90 days ahead (tasks
    -- with time entries first), size them by their hours, and put them in the column their dates
    -- imply. Skipped when the six board columns are not all present and active.
    SELECT array_agg(w."Id" ORDER BY o.n) INTO workflow_ids
    FROM generate_series(1, 6) AS o(n)
    CROSS JOIN LATERAL (SELECT "Id" FROM "Workflows" WHERE "Active" AND "Name" = workflow_names[o.n] ORDER BY "Order", "Id" LIMIT 1) w;
    IF cardinality(workflow_ids) = 6 THEN
        WITH placed AS (
            SELECT id, hours,
                   date_trunc('day', now(), 'UTC') - interval '270 days' + interval '9 hours'
                       + floor((row_number() OVER (PARTITION BY project_id ORDER BY has_entries DESC, id) - 1) * 360.0
                               / count(*) OVER (PARTITION BY project_id)) * interval '1 day' AS start_date
            FROM demo_names_task)
        UPDATE demo_names_task t
        SET start_date = placed.start_date,
            end_date = placed.start_date + (greatest(ceil(placed.hours / 4.0), 1) - 1) * interval '1 day' + interval '8 hours'
        FROM placed WHERE placed.id = t.id;
        UPDATE demo_names_task SET workflow_order = CASE
            WHEN end_date < now() THEN 6
            WHEN start_date > now() + interval '30 days' THEN 1
            WHEN start_date > now() THEN 2
            WHEN now() - start_date < (end_date - start_date) * 0.5 THEN 3
            WHEN now() - start_date < (end_date - start_date) * 0.8 THEN 4
            ELSE 5 END;
    ELSE
        RAISE NOTICE 'Demo workspace names: task schedules kept because the six board columns are not all active.';
    END IF;

    UPDATE "Assignments" x
    SET "Name" = t.name, "Description" = t.description,
        "StartDate" = COALESCE(t.start_date, x."StartDate"),
        "EndDate" = COALESCE(t.end_date, x."EndDate"),
        "WorkflowId" = COALESCE(workflow_ids[t.workflow_order], x."WorkflowId")
    FROM demo_names_task t WHERE t.id = x."Id";
    GET DIAGNOSTICS changed = ROW_COUNT;
    summary := summary || format(', Assignments=%s', changed);

    -- Time entries: a work-log line matching the task, dated inside the task's worked window.
    CREATE TEMP TABLE demo_names_entry (id uuid PRIMARY KEY, description text, keep_date timestamptz);
    INSERT INTO demo_names_entry
    SELECT e.id, format(c.val, t.defect, t.subject) || '.',
           CASE WHEN t.start_date IS NULL THEN e.keep_date
                WHEN least(t.end_date, now()) <= t.start_date THEN t.start_date
                ELSE least(greatest(
                         date_trunc('day', t.start_date + (least(t.end_date, now()) - t.start_date) * ((e.q + 1)::float8 / (e.c + 1)), 'UTC')
                             + (9 + abs(hashtext(e.id::text)) % 8) * interval '1 hour',
                         t.start_date), least(t.end_date, now())) END
    FROM (SELECT "Id" AS id, "AssignmentId" AS task_id, "KeepDate" AS keep_date,
                 row_number() OVER (PARTITION BY "AssignmentId" ORDER BY "KeepDate", "Id") - 1 AS q,
                 count(*) OVER (PARTITION BY "AssignmentId") AS c
          FROM "Appointments"
          WHERE "Description" IS NULL OR btrim("Description") = '' OR "Description" ~ hacker_phrase) e
    JOIN demo_names_task t ON t.id = e.task_id
    JOIN demo_names_catalog c ON c.kind = 'entry' AND c.grp = t.role AND c.idx = e.q % 8;

    ANALYZE demo_names_entry;
    UPDATE "Appointments" x SET "Description" = e.description, "KeepDate" = e.keep_date
    FROM demo_names_entry e WHERE e.id = x."Id";
    GET DIAGNOSTICS changed = ROW_COUNT;
    summary := summary || format(', Appointments=%s', changed);

    -- Blocker notes repeat the impediment with a follow-up.
    WITH noted AS (
        SELECT x."Id", format(c.val, i."Name") AS description
        FROM (SELECT "Id", "ImpedimentId", row_number() OVER (ORDER BY "Id") - 1 AS n
              FROM "AssignmentImpediments"
              WHERE "Description" IS NULL OR btrim("Description") = '' OR "Description" ~ hacker_phrase) x
        JOIN "Impediments" i ON i."Id" = x."ImpedimentId"
        JOIN demo_names_catalog c ON c.kind = 'note' AND c.idx = x.n % 4)
    UPDATE "AssignmentImpediments" t SET "Description" = noted.description FROM noted WHERE noted."Id" = t."Id";
    GET DIAGNOSTICS changed = ROW_COUNT;
    summary := summary || format(', AssignmentImpediments=%s', changed);

    -- Logins: first.last at the domain of the user's first organization (reserved .example TLD).
    WITH people AS (
        SELECT u."Id",
               regexp_replace(btrim(regexp_replace(lower(u."Name"), '[^a-z ]', '', 'g')), ' +', '.', 'g') AS local_part,
               COALESCE((SELECT btrim(regexp_replace(lower(o."Name"), '[^a-z0-9]+', '-', 'g'), '-')
                         FROM "UserProjects" up
                         JOIN "Projects" p ON p."Id" = up."ProjectId"
                         JOIN "Organizations" o ON o."Id" = p."OrganizationId"
                         WHERE up."UserId" = u."Id"
                         ORDER BY up."Active" DESC, up."Id" LIMIT 1), 'cpnucleo') || '.example' AS domain
        FROM "Users" u WHERE u."Login" ~ '^learner-[0-9]+$'),
    candidates AS (
        SELECT "Id", CASE WHEN local_part = '' THEN 'member' ELSE local_part END AS local_part, domain,
               row_number() OVER (PARTITION BY CASE WHEN local_part = '' THEN 'member' ELSE local_part END, domain ORDER BY "Id") AS rn
        FROM people),
    logins AS (
        SELECT c."Id",
               CASE WHEN c.rn = 1 AND NOT EXISTS (
                        SELECT 1 FROM "Users" o WHERE o."Login" !~ '^learner-[0-9]+$'
                          AND lower(btrim(o."Login")) = c.local_part || chr(64) || c.domain)
                    THEN c.local_part || chr(64) || c.domain
                    ELSE c.local_part || '.' || substr(md5(c."Id"::text), 1, 6) || chr(64) || c.domain END AS login
        FROM candidates c)
    UPDATE "Users" t SET "Login" = logins.login FROM logins WHERE logins."Id" = t."Id";
    GET DIAGNOSTICS changed = ROW_COUNT;
    summary := summary || format(', Users=%s', changed);

    DROP TABLE pg_temp.demo_names_catalog, pg_temp.demo_names_org, pg_temp.demo_names_project, pg_temp.demo_names_type, pg_temp.demo_names_task, pg_temp.demo_names_entry;
    RAISE NOTICE 'Demo workspace names: %', summary;
END
$demo_workspace_names$;
