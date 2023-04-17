# Cpnucleo 
A sample solution that implements the best praticles when building a .NET projects

[![CodeFactor](https://www.codefactor.io/repository/github/jonathanperis/cpnucleo/badge)](https://www.codefactor.io/repository/github/jonathanperis/cpnucleo) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=jonathanperis_cpnucleo&metric=alert_status)](https://sonarcloud.io/dashboard?id=jonathanperis_cpnucleo)

# Architecture 

[![](https://mermaid.ink/img/pako:eNqlk0uP2jAUhf9K5CViyKtAYFEJAkxnQYuAPlRgcXGcYDXYqX0zLQP899qBAhp1povJyj7X57v3WPGeUJkw0iVpLn_RDSh05oOlcMwXF6KkOZON8ZfYubt7f5goiXJdpqmzNCdqY8g4_SS4FDXn4PQWl_P300m8esaYQMZ0RZkOZ_MTYMpSjrWD03_d2s_hSaoXvPHVO-aCbyHvTR7OhGecXlHknAKagRtzprEifhbcJDY7fQ4lf7rfSiO6o7xkAntaM2Ut2u3LrNQ26nDxL-a5aa_CjlnCAc3UJ-h5919z_y3m-E3mS0nRDUdGsVTsek236u11fWRoS1Z54dpe7XzqPaxaDADB6VHK9Jk9HDmxVMwCRlfAg0gVaFRlNcvqljAUyJEz7camL4JA7Y6AmvRG-_vLFgVT-c4y76_MgdwCv84zOkX-Ojs4HxYTqTFTBmDnW4NmK1InW6aMIzHPZm8tS4IbtmVL0jXLBNSPJVmKozkHJcrZTlDSNQOzOimLBJANOGQKtqSbQq4v6jDhZtSLmEtImNnuCe4K-0AzrtEgqRQpz6xeqtzIG8RCd13XlhsZx025blC5dTVP7GvePHZabitoRRCErNUOoRmGCV37nSgN3vlp0vb8AMjxWCcFCEv9TbpRw4s8L2xHnXbkhYHXrJOdUYOGH4WtVjNoNiPf85vG8ySlieFX5u_V2uY8_gGKF3Do?type=png)](https://mermaid.live/edit#pako:eNqlk0uP2jAUhf9K5CViyKtAYFEJAkxnQYuAPlRgcXGcYDXYqX0zLQP899qBAhp1povJyj7X57v3WPGeUJkw0iVpLn_RDSh05oOlcMwXF6KkOZON8ZfYubt7f5goiXJdpqmzNCdqY8g4_SS4FDXn4PQWl_P300m8esaYQMZ0RZkOZ_MTYMpSjrWD03_d2s_hSaoXvPHVO-aCbyHvTR7OhGecXlHknAKagRtzprEifhbcJDY7fQ4lf7rfSiO6o7xkAntaM2Ut2u3LrNQ26nDxL-a5aa_CjlnCAc3UJ-h5919z_y3m-E3mS0nRDUdGsVTsek236u11fWRoS1Z54dpe7XzqPaxaDADB6VHK9Jk9HDmxVMwCRlfAg0gVaFRlNcvqljAUyJEz7camL4JA7Y6AmvRG-_vLFgVT-c4y76_MgdwCv84zOkX-Ojs4HxYTqTFTBmDnW4NmK1InW6aMIzHPZm8tS4IbtmVL0jXLBNSPJVmKozkHJcrZTlDSNQOzOimLBJANOGQKtqSbQq4v6jDhZtSLmEtImNnuCe4K-0AzrtEgqRQpz6xeqtzIG8RCd13XlhsZx025blC5dTVP7GvePHZabitoRRCErNUOoRmGCV37nSgN3vlp0vb8AMjxWCcFCEv9TbpRw4s8L2xHnXbkhYHXrJOdUYOGH4WtVjNoNiPf85vG8ySlieFX5u_V2uY8_gGKF3Do)

# Build Status

- [Cpnucleo Pages](https://cpnucleo-pages.azurewebsites.net) (Uses API Cpnucleo as backend)

[![Build status](https://dev.azure.com/peris-studio/cpnucleo/_apis/build/status/Cpnucleo%20-%20Pages%20-%20ASP.NET%20Core%20-%20CI)](https://dev.azure.com/peris-studio/cpnucleo/_build/latest?definitionId=7)

- [Cpnucleo MVC](https://cpnucleo-mvc.azurewebsites.net) (Uses gRPC Cpnucleo as backend)

[![Build status](https://dev.azure.com/peris-studio/cpnucleo/_apis/build/status/Cpnucleo%20-%20MVC%20-%20ASP.NET%20Core%20-%20CI)](https://dev.azure.com/peris-studio/cpnucleo/_build/latest?definitionId=9)

- [API Cpnucleo](https://api-cpnucleo.azurewebsites.net/swagger)

[![Build status](https://dev.azure.com/peris-studio/cpnucleo/_apis/build/status/API%20-%20Cpnucleo%20-%20ASP.NET%20Core%20-%20CI)](https://dev.azure.com/peris-studio/cpnucleo/_build/latest?definitionId=8)

- [gRPC Cpnucleo](https://grpc-cpnucleo.azurewebsites.net)

[![Build status](https://dev.azure.com/peris-studio/cpnucleo/_apis/build/status/GRPC%20-%20Cpnucleo%20-%20ASP.NET%20Core%20-%20CI)](https://dev.azure.com/peris-studio/cpnucleo/_build/latest?definitionId=11)

- [Cpnucleo Blazor](https://cpnucleo-blazor.azurewebsites.net)

[![Build status](https://dev.azure.com/peris-studio/cpnucleo/_apis/build/status/Cpnucleo%20-%20Blazor%20-%20ASP.NET%20Core%20-%20CI)](https://dev.azure.com/peris-studio/cpnucleo/_build/latest?definitionId=17)
