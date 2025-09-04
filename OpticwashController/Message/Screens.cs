namespace OpticwashController.Message;

public enum Screens
{
    Unknown = -1,
    Standby,
    InsertCashOrCard,
    WaitForApproval,
    CashOrCardApproval,
    InsertItemToWash,
    WashCycle,
    DryCycle,
    CycleComplete,
    ServiceMode,
    ErrorScreen,
    SelectItemToWash,
    InsertEyewearToWash,
    InsertJewelryToWash,
    InsertPhoneToWash,
}