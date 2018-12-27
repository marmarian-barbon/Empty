/// <summary>
/// Предоставляет возможность создания графической составляющей <see cref="Suppressor"/>.
/// </summary>
public interface ITowerBuilder
{
    /// <summary>
    /// Создает графическую составляющую <paramref name="suppressor"/>.
    /// </summary>
    /// <param name="suppressor"></param>
    void Build(Suppressor suppressor);

    /// <summary>
    /// Уничтожает графическую составляющую.
    /// </summary>
    void Destroy();
}
