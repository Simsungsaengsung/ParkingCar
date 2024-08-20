using System;

public static class Events
{
    public static OptionButtonClickEvent OptionButtonClickEvent = new();
    public static SceneChangeEvent SceneChangeEvent = new();
    public static StartParkingEvent StartParkingEvent = new();
}

public class OptionButtonClickEvent : GameEvent
{
    public bool open;
    public bool timeStop;
}

public class SceneChangeEvent : GameEvent
{
    public Action callBack;
}

public class StartParkingEvent : GameEvent
{
    
}