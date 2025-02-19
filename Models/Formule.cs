using System.Data;
using System.Text.Json;
using System.Threading.Tasks;
using LimsFrontEnd.Utils;

namespace LimsTravauxService.Models;

public class Formule
{

    private Formule DtoToFormule(FormuleDto employeDto)
    {
        Formule result = new Formule();
        string dtoAsJson = JsonSerializer.Serialize(employeDto);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        result = JsonSerializer.Deserialize<Formule>(dtoAsJson, options);
        return result;
    }

    public Formule Evaluer(FormuleDto formuleDto)
    {
        Formule result = new Formule();
        result = result.DtoToFormule(formuleDto);
        string formuleValue = formuleDto.FormuleChosed.Replace(",", ".");
        foreach(string key in formuleDto.Variable.Keys)
        {
            formuleValue = formuleValue.Replace(key, formuleDto.Variable[key].ToString());
        }
        var tarif = new DataTable().Compute(formuleValue, null);
        result.Tarif = Convert.ToDouble(tarif);

        return result;
    }
    public Dictionary<string, decimal> Variable { get; set; } = new Dictionary<string, decimal>();
    public Double? Tarif { get; set; }
    public string? FormuleString { get; set;}

}