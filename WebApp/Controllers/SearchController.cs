using DotnetGeminiSDK.Client;
using DotnetGeminiSDK.Config;
using DotnetGeminiSDK.Model;
using Microsoft.AspNetCore.Mvc;
using WebApp.Resources;

namespace WebApp.Controllers;

[ApiController]
[Route("[controller]")]
public class SearchController : ControllerBase
{
    private const string Prompt = @"
        identifique a raça, cor, características e todas as outras informações que puder extrair sobre o cachorro na foto.
        o retorno deve ser no seguinte formato:
        {
            'valid': true,
            'raca': '',
            'cor': '',
            'pelagem': '',
            'olhos': '',
            'focinho': '',
            'tamanho': '',
            'idade': '',
            'outrasInformacoes': 'outras informações sobre a foto'
        }
        se a imagem não conter um cachorro, ou o cachorro da foto for um desenho, retorne o seguinte json:
        {
            'valid': false
        }
    ";

    [HttpPost]
    public async Task<IActionResult> Upload([FromForm] FileUploadResource fileUpload)
    {
        if (fileUpload?.File == null)
            return BadRequest();
        
        string imageBase64 = await TranformImageToArrayOfBytes(fileUpload);

        var geminiClient = new GeminiClient(new GoogleGeminiConfig
        {
            ApiKey = "AIzaSyBsgCgtPDG9Jwo-lmDpS8KXX9obpb0x3Fk"
        });

        var response = await geminiClient.ImagePrompt(Prompt, imageBase64, ImageMimeType.Jpeg);

        //TODO: Salvar consulta e resposta
        
        return Ok(response);
    }

    private static async Task<string> TranformImageToArrayOfBytes(FileUploadResource fileUpload)
    {
        var fileStream = new MemoryStream();
        await fileUpload.File.CopyToAsync(fileStream);
        fileStream.Position = 0;
        byte[] fileBytes = fileStream.ToArray();
        string base64Image = Convert.ToBase64String(fileBytes);
        return base64Image;
    }
}