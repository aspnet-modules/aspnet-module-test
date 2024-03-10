using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Bogus.DataSets;

namespace AspNet.Module.Test.Extensions;

[SuppressMessage("ReSharper", "ArgumentsStyleNamedExpression")]
public static class AddressExtensions
{
    public static string RussiaDistrict(this Address address) => address.Random.ListItem(Data.DistrictList);

    public static string RussiaRegion(this Address address) => address.Random.ListItem(Data.RegionList);

    public static string RussiaCity(this Address address) => address.Random.ListItem(Data.CityList);

    public static string RussiaLocality(this Address address) => address.Random.ListItem(Data.LocalityList);

    public static string RussiaStreet(this Address address) => address.Random.ListItem(Data.StreetList);

    public static string RussiaFullAddress(this Address address)
    {
        var region = address.RussiaRegion();
        var district = address.RussiaDistrict();
        var city = address.RussiaCity();
        var street = address.RussiaStreet();
        var house = address.Random.Number(1, 36).ToString();
        var apartment = address.Random.Number(1, 24).ToString();
        return GenerateFullAddress(
            region: region,
            district: district,
            city: city,
            street: street,
            house: house,
            apartment: apartment
        );
    }

    public static string GenerateFullAddress(string region, string district, string city, string street,
        string house, string apartment) =>
        NormalizeAddress($"{region}, {district}, {city}, {street} {house}, {apartment}");

    public static string GenerateFullAddress(this Address address)
    {
        var region = address.RussiaRegion();
        var district = address.RussiaDistrict();
        var city = address.RussiaCity();
        var street = address.RussiaStreet();
        var house = address.Random.Number(1, 36).ToString();
        var apartment = address.Random.Number(1, 24).ToString();
        return NormalizeAddress($"{region}, {district}, {city}, {street} {house}, {apartment}");
    }

    private static string NormalizeAddress(string address) =>
        // m - bug in Bogus
        Regex.Replace(address, @"(&|'|\(|\)|<|>|#|-|—|m)", "");

    public static class Data
    {
        public static readonly string[] RegionList =
        {
            "Москва"
        };

        public static readonly string[] DistrictList =
        {
            "Бирюлево Восточное",
            "Бирюлево Западное"
        };

        public static readonly string[] CityList =
        {
            "Москва"
        };

        public static readonly string[] LocalityList =
        {
            "Москва"
        };

        public static readonly string[] StreetList =
        {
            "Пришвина",
            "Пушкина",
            "Маяковского"
        };
    }
}