using System;

namespace dotnet_cpnucleo_pages.Extension
{
    public static class DecimalExtensions 
    {
        public static decimal Round(this decimal input, int places) 
        {
            if (places < 0) return input; 
            
            decimal multiplier = 1; 
            
            for (int i = 0; i < places; ++i) multiplier *= 10; 
            
            return (Math.Round((input * multiplier) / multiplier, MidpointRounding.AwayFromZero)); 
        }        

        public static decimal RoundCeiling(this decimal input, int places) 
        {
            if (places < 0) return input; 
            
            decimal multiplier = 1; 
            
            for (int i = 0; i < places; ++i) multiplier *= 10; 
            
            return (Math.Ceiling(input * multiplier) / multiplier); 
        }
    }    
}