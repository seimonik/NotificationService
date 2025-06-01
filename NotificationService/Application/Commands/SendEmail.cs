using MediatR;
using NotificationService.Application.Models;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;

namespace NotificationService.Application.Commands;

public static class SendEmail
{
	public record Command(SendBookingNotificationRequest Request) : IRequest<Unit>;

	internal class Handler : IRequestHandler<Command, Unit>
	{
		public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
		{
			try
			{
				MailAddress from = new("bookingservicessu@mail.ru", "SSU Booking");
				MailAddress to = new(request.Request.Client.Email);

				var filePath = Path.Combine("Assets", "MessageTemplate.html");
				var messageTemplate = File.ReadAllText(filePath);

				var values = new Dictionary<string, string>
				{
					{ "ClientName", request.Request.Client.FullName },
					{ "HotelName", request.Request.Room.Hotel.Name },
					{ "BookingNumber", request.Request.BookingId.ToString() },
					{ "CheckInDate", request.Request.CheckInDate.ToString("dd.MM.yyyy") },
					{ "CheckOutDate", request.Request.CheckOutDate.ToString("dd.MM.yyyy") },
					{ "RoomNumber", request.Request.Room.Number.ToString() },
					{ "TotalPrice", request.Request.TotalPrice.ToString("N2") }
				};

				var messageBody = Regex.Replace(messageTemplate, @"\{\{(\w+)\}\}", match =>
				{
					string key = match.Groups[1].Value;
					return values.TryGetValue(key, out var val) ? val : match.Value;
				});
				var messageSubject = $"Забронирован номер в отеле {request.Request.Room.Hotel.Name}";

				MailMessage mailMessage = new(from, to)
				{
					Subject = messageSubject,
					Body = messageBody,
					IsBodyHtml = true
				};

				SmtpClient smtp = new("smtp.mail.ru", 587)
				{
					Credentials = new NetworkCredential("bookingservicessu@mail.ru", "qjy7LdzqlbrWoIqcYXVR"),
					EnableSsl = true
				};
				await smtp.SendMailAsync(mailMessage, cancellationToken);
			} catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return Unit.Value;
		}
	}
}
