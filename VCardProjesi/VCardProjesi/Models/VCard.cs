namespace VCardProjesi.Models;

public class VCard
{
    public Name Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public Location Location { get; set; }

    public string VCardDonusturme()
    {
        string res = 
            $"BEGIN  :  VCARD\r\n" +
            $"VERSION:  3.0\r\n" +
            $"N      :  {Name.First};{Name.Last}\r\n" +
            $"EMAIL  :  {Email}\r\n" +
            $"TEL    :  {Phone}\r\n" +
            $"ADR    :  {Location.City};{Location.Country}\r\n" +
            $"END    :  VCARD\r\n";

        return res;
    }
}

public class Name
{
    public string First { get; set; }
    public string Last { get; set; }
}

public class Location
{
    public string City { get; set; }
    public string Country { get; set; }
}