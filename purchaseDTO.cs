using System.ComponentModel.DataAnnotations;

public class purchaseDTO{
    /// <summary>
    /// Unique identifier for the eBook.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// quantity of the eBook.
    /// </summary>
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Cantidad debe ser un número positivo.")]
    public int quantity { get; set; }

    /// <summary>
    /// quantity of the eBook.
    /// </summary>
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "El dinero debe ser un número positivo, nada es regalo en la vida.")]
    public int pay { get; set; }
}
