using System.ComponentModel.DataAnnotations;

public class stockDTO{
    /// <summary>
    /// Stock of the eBook.
    /// </summary>
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser un número positivo.")]
    public int Stock { get; set; }
}