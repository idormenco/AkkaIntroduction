namespace Akka.Net.AdvancedExample.SharedMessages
{
    public class PrintMeMessage
    {
        public string Name { get; }
        public GenderEnum Gender { get; }
        public int Capital { get; }
        public string Path { get; }

        public PrintMeMessage(string name, GenderEnum gender, int capital, string path)
        {
            Name = name;
            Gender = gender;
            Capital = capital;
            Path = path;
        }
    }

    public class GothcaMessage
    {
        public static GothcaMessage Instance => new GothcaMessage();
    }

    public class GimmyTaxMessage
    {
        public static GimmyTaxMessage Instance => new GimmyTaxMessage();
    }

    public class CollectTaxesMessage
    {
        public static CollectTaxesMessage Instance => new CollectTaxesMessage();
    }

    public class TaxIncomeMessage
    {
        public int Amount { get; }

        public TaxIncomeMessage(int amount)
        {
            Amount = amount;
        }
    }

    
}
