namespace Web_153504_Padvalnikau.API.Services.ProductService;
using Web_153504_Padvalnikau.Domain.Entities;
using Web_153504_Padvalnikau.Domain.Models;

public interface IProductService
{
    /// <summary>
    /// Получение списка всех объектов
    /// </summary>
    /// <param name="categoryNormalizedName">нормализованное имя категории для фильтрации</param>
    /// <param name="pageNo">номер страницы списка</param>
    /// <returns></returns>
    public Task<ResponseData<ListModel<Sneaker>>> GetSneakerListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3);

    /// <summary>
    /// Поиск объекта по Id
    /// </summary>
    /// <param name="id">Идентификатор объекта</param>
    /// <returns></returns>
    public Task<ResponseData<Sneaker>> GetByIdAsync(int id);
        
    /// <summary>
    /// Обновление объекта
    /// </summary>
    /// <param name="id">Id изменяемомго объекта</param>
    /// <param name="product">объект с новыми параметрами</param>
    /// <returns></returns>
    public Task UpdateAsync(int id, Sneaker sneaker);
        
    /// <summary>
    /// Удаление объекта
    /// </summary>
    /// <param name="id">Id удаляемомго объекта</param>
    /// <returns></returns>
    public Task DeleteAsync(int id);

    /// <summary>
    /// Создание объекта
    /// </summary>
    /// <param name="product">Новый объект</param>
    /// <returns>Созданный объект</returns>
    public Task<ResponseData<Sneaker>> CreateAsync(Sneaker product);
        
    /// <summary>
    /// Сохранить файл изображения для объекта
    /// </summary>
    /// <param name="id">Id объекта</param>
    /// <param name="formFile">файл изображения</param>
    /// <returns>Url к файлу изображения</returns
    public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile);
}