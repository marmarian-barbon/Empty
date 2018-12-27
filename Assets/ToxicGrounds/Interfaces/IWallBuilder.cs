/// <summary>
/// Предоставляет возможность создания графической составляющей <see cref="Wall"/>.
/// </summary>
public interface IWallBuilder
{
    /// <summary>
    /// Создает графическую составляющую <paramref name="wall"/>.
    /// </summary>
    /// <param name="wall"></param>
    void Build(Wall wall);

    /// <summary>
    /// Уничтожает графическую составляющую.
    /// </summary>
    void Destroy();
}
