namespace Application.Models.Options
{
    internal class Option
    {
        public string Name { get; set; }

        public string ConnectionString { get; set; }

        public string MainScriptName { get; set; }

        public string Tree { get; set; }

        public string GeneralScript { get; set; }

        /// <summary>
        /// потребление ХВС<ГВС более чем на ХХХ значение
        /// </summary>
        public string AnalyticHvsLessGvs { get; set; }

        /// <summary>
        /// отсутствие потребления ЭЭ при наличии потребления ХВС/ГВС и наоборот
        /// </summary>
        public string AnalyticNoEe { get; set; }

        /// <summary>
        /// отсутствие данных по объекту учета в течении определенного периода времени;
        /// </summary>
        public string AnalyticNoMeasures { get; set; }

        /// <summary>
        /// отсутствие потребления ХВС, при наличии ГВС
        /// </summary>
        public string AnalyticNoHvs { get; set; }

        /// <summary>
        /// аномальный расход с возможностью выставления верхнего и нижнего уровня диапазона.
        /// </summary>
        public string AnalyticAbnormal { get; set; }

        /// <summary>
        /// Отрицательная разница ХВС/ГВС
        /// </summary>
        public string AnalyticNegative { get; set; }

    }
}
