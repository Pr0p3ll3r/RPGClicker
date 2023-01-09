[System.Serializable]
public class SaveItem
{
    public int ID;
    public int amount;
    public SaveItem()
    {
        ID = -1;
        amount = 0;
    }
    public SaveItem(int _ID, int _amount)
    {
        ID = _ID;
        amount = _amount;
    }
}
