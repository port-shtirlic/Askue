namespace Application.Models.Options
{
    /// <summary>
    /// Какой скрипт?
    /// </summary>
    internal enum OptionScriptType
    {
        /// <summary>
        /// Дерево ПУ
        /// </summary>
        Tree = 1,

        /// <summary>
        /// Показания ПУ
        /// </summary>
        Main = 2,

        /// <summary>
        /// потребление ХВС<ГВС более чем на ХХХ значение
        /// </summary>
        AnalyticHvsLessGvs = 3,

        /// <summary>
        /// отсутствие потребления ЭЭ при наличии потребления ХВС/ГВС и наоборот
        /// </summary>
        AnalyticNoEe = 4,

        /// <summary>
        /// отсутствие данных по объекту учета в течении определенного периода времени;
        /// </summary>
        AnalyticNoMeasures = 5,

        /// <summary>
        /// отсутствие потребления ХВС, при наличии ГВС
        /// </summary>
        AnalyticNoHvs = 6,

        /// <summary>
        /// аномальный расход с возможностью выставления верхнего и нижнего уровня диапазона.
        /// </summary>
        AnalyticAbnormal = 7,

        /// <summary>
        /// Отрицательная разница ХВС/ГВС
        /// </summary>
        AnalyticNegative = 8,

        /// <summary>
        /// Общая часть скриптов
        /// </summary>
        GeneralScript = 9
    }
}