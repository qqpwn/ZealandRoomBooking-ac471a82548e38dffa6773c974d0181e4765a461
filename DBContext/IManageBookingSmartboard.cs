using System.Collections.Generic;
using ModelKlasser;

namespace DBContext
{
    public interface IManageBookingSmartboard
    {
        List<BookingSmartboard> GetAllBookingSmartboard();

        BookingSmartboard GetBookingSmartboardFromId(int bookingSmartboardId);

        bool CreateBookingSmartboard(BookingSmartboard bookingSmartboard);

        bool UpdateBookingSmartboard(BookingSmartboard bookingSmartboard, int bookingSmartboardId);

        BookingSmartboard DeleteBookingSmartboard(int bookingSmartboardId);
    }
}