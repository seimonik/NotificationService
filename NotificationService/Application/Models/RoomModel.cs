namespace NotificationService.Application.Models;

public class RoomModel
{
	public int Number { get; set; }
	public HotelModel Hotel { get; set; } = null!;
}
