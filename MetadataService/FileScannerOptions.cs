﻿namespace MetadataService;

//класс представляющий собой секцию из appsettings.json
public sealed class FileScannerOptions
{
    //путь к директории
    public string Path { get; set; }
    //маска файла
    public string SearchPattern { get; set; }
    //интервал времени
    public int Interval { get; set; }
}