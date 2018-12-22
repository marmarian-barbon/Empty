public interface IBuildingUIState
{
    BuildingUI BuildingUi { get; }

    /// <summary>
    /// Обновление на новом кадре.
    /// </summary>
    /// <returns></returns>
    IBuildingUIState Update();

    /// <summary>
    /// Отменить текущее состояние и вернуть его к <see cref="BuildingUIFree"/>.
    /// </summary>
    IBuildingUIState Cancel();
}
