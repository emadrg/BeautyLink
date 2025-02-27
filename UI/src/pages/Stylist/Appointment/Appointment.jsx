import interactionPlugin from "@fullcalendar/interaction";
import FullCalendar from "@fullcalendar/react";
import timeGridPlugin from "@fullcalendar/timegrid";
import { Box, Modal } from "@mui/material";
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import CustomLoader from "../../../components/utility/CustomLoader";
import useAppointmentService from "../../../services/AppointmentService";
import useScheduleService from "../../../services/ScheduleService";
import useStylistService from "../../../services/StylistService";
import useUnavailableTimeServce from "../../../services/UnavailableTimeServce";
import "../../../styles/pages/appointment.scss";
import Stylist from "../Stylist";
import Service from "./Service";

const Appointment = () => {
  const [stylist, setStylist] = useState("");
  const [selectedServices, setSelectedServices] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [schedule, setSchedule] = useState([]);
  const [unavailableTime, setUnavailableTime] = useState([]);
  const [appointment, setAppointment] = useState();
  const [modalIsOpen, setModalIsOpen] = useState(false);
  const [stylistBookedTime, setStylistBookedTime] = useState([]);
  const [totalUnavailableBookingTime, setTotalUnavailableBookingTime] =
    useState([]);
  const navigate = useNavigate();

  const { getStylistWithServices } = useStylistService();
  const { getScheduleByStylistId } = useScheduleService();
  const { getUnavailableTimeByStylistId } = useUnavailableTimeServce();
  const { createAppointment } = useAppointmentService();
  const { getAppointmentsByStylistId } = useAppointmentService();

  const style = {
    position: "absolute",
    top: "50%",
    left: "50%",
    transform: "translate(-50%, -50%)",
    width: 350,
    height: 150,
    bgcolor: "background.paper",
    border: "2px solid #000",
    boxShadow: 24,
    p: 4,
    paddingLeft: 5,
  };

  const appointmentColor = "#e3c3d2";
  const unavailableTimeColor = "#eee";

  const { id: stylistId } = useParams();

  const fetchStylist = async (stylistId) => {
    try {
      let stylistFromApi = await getStylistWithServices(stylistId);
      setStylist(stylistFromApi);
      setIsLoading(false);
    } catch (e) {
      console.log(e);
      setIsLoading(false);
    }
  };

  const fetchSchedule = async (stylistId) => {
    try {
      setSchedule(await getScheduleByStylistId(stylistId));
      setIsLoading(false);
    } catch (e) {
      console.log(e);
      setIsLoading(false);
    }
  };

  const handleModalState = () => {
    setModalIsOpen(!modalIsOpen);
  };

  const handlePageReload = () => {
    navigate("/");
  };

  const fetchUnavailableTime = async (stylistId) => {
    try {
      let nonWorkingIntervals = await getUnavailableTimeByStylistId(stylistId);
      nonWorkingIntervals = nonWorkingIntervals.map((interval) => ({
        start: interval.startDate,
        end: interval.endDate,
        color: unavailableTimeColor,
        textColor: unavailableTimeColor,
      }));
      setIsLoading(false);
      return nonWorkingIntervals;
    } catch (e) {
      console.log(e);
      setIsLoading(false);
    }
  };

  const fetchStylistBookedIntervals = async (stylistId) => {
    try {
      let bookedIntervals = await getAppointmentsByStylistId(stylistId);
      bookedIntervals = bookedIntervals.map((bookedInterval) => ({
        start: bookedInterval.startDate,
        end: bookedInterval.endDate,
        color: unavailableTimeColor,
        textColor: unavailableTimeColor,
      }));
      setIsLoading(false);
      return bookedIntervals;
    } catch (e) {
      console.log(e);
      setIsLoading(false);
    }
  };

  const handleTimeSlotsSelectUnavailableTime = (
    startDateISOString,
    endDateISOString
  ) => {
    let filteredUnavailableTimeByCurrentDate =
      totalUnavailableBookingTime.filter(
        (timeSlot) =>
          new Date(timeSlot.start).getDate() ===
          new Date(startDateISOString).getDate()
      );

    if (filteredUnavailableTimeByCurrentDate.length == 0) {
      setAppointment({
        start: startDateISOString,
        end: endDateISOString,
        color: appointmentColor,
      });
    }

    filteredUnavailableTimeByCurrentDate.forEach((timeSlot) => {
      if (
        endDateISOString <= timeSlot.start ||
        timeSlot.end <= startDateISOString
      ) {
        setAppointment({
          start: startDateISOString,
          end: endDateISOString,
          color: appointmentColor,
        });
      } else {
        setAppointment(undefined);
      }
    });
  };

  const handleTimeSlotsSelectSchedule = (
    startDateISOString,
    endDateISOString
  ) => {
    let filterScheduleByWeekday = schedule.filter(
      (s) => s.weekDayId == new Date(startDateISOString).getDay() + 1
    );

    filterScheduleByWeekday.forEach((s) => {
      let startDateTime = new Date(startDateISOString)
        .toTimeString()
        .split(" ")[0];

      let endDateTime = new Date(endDateISOString).toTimeString().split(" ")[0];

      if (s.startTime <= startDateTime && s.endTime >= endDateTime) {
        handleTimeSlotsSelectUnavailableTime(
          startDateISOString,
          endDateISOString
        );
      } else {
        setAppointment(undefined);
      }
    });
  };

  const selectTimeSlot = async (e) => {
    let inputDate = new Date(e.dateStr);
    const addHours = (n) => n * 60 * 60 * 1000;

    let totalTime =
      selectedServices.reduce(
        (accumulator, currentValue) =>
          accumulator + currentValue.durationMinutes,
        0
      ) / 60;

    if (totalTime === 0) return;

    let startDate = new Date(inputDate.getTime() + addHours(3));
    let endDate = new Date(startDate.getTime() + addHours(totalTime));

    let currentDate = new Date(Date.now());
    let localCurrentDate = new Date(currentDate.getTime() + addHours(3));
    if (startDate < localCurrentDate) {
      return null;
    }

    let startDateISOString = startDate.toISOString().split(".")[0];
    let endDateISOString = endDate.toISOString().split(".")[0];
    handleTimeSlotsSelectSchedule(startDateISOString, endDateISOString);
  };

  const handleAddService = (e) => {
    let condition = selectedServices
      .map((s) => s.serviceId)
      .includes(e.serviceId);

    if (!condition) {
      setSelectedServices([...selectedServices, e]);
    } else {
      let newselectedServices = selectedServices.filter(
        (service) => service.serviceId != e.serviceId
      );
      setSelectedServices(newselectedServices);
    }
  };

  const handleConfirmAppointment = async () => {
    let appointmentObject = {
      startDate: appointment.start,
      endDate: appointment.end,
      services: selectedServices.map((s) => ({
        serviceId: s.serviceId,
        stylistId: stylistId,
      })),
    };
    try {
      await createAppointment(appointmentObject);
      handleModalState();
    } catch (err) {
      console.log(err);
    }
  };

  const mergeUnavailableTimeArrays = async (stylistId) => {
    let unavailableTimes = await fetchUnavailableTime(stylistId);
    let stylistBookedTimes = await fetchStylistBookedIntervals(stylistId);
    const mergedArr = [...unavailableTimes, ...stylistBookedTimes];

    setUnavailableTime(unavailableTimes);
    setStylistBookedTime(stylistBookedTimes);
    setTotalUnavailableBookingTime(mergedArr);
  };

  useEffect(() => {
    fetchStylist(stylistId);
    fetchSchedule(stylistId);
    mergeUnavailableTimeArrays(stylistId);
  }, []);

  if (isLoading) {
    return <CustomLoader></CustomLoader>;
  }

  return (
    <div>
      <div className="stylist-and-appointment-details">
        <Stylist displayReview={false}></Stylist>
        <div className="appointment-details">
          <h3 style={{ color: "#b05f85" }}>Appointment details</h3>
          <div>
            Total time:{" "}
            {selectedServices.reduce(
              (accumulator, currentValue) =>
                accumulator + currentValue.durationMinutes,
              0
            )}{" "}
            mins
          </div>{" "}
          <div>
            Total price: ${""}
            {selectedServices.reduce(
              (acc, currentValue) => acc + currentValue.price,
              0
            )}
          </div>
          <button
            className="confirm-button"
            style={{ marginTop: 20 }}
            onClick={handleConfirmAppointment}
          >
            Confirm appointment
          </button>
          <div>
            <Modal open={modalIsOpen} onClose={() => handleModalState()}>
              <Box sx={{ ...style }}>
                <div>Appointment successfully created!</div>
                <button
                  style={{ marginLeft: 90, marginTop: 20 }}
                  onClick={() => handlePageReload()}
                >
                  ok
                </button>
              </Box>
            </Modal>
          </div>
        </div>
      </div>
      <div className="service-options">
        {stylist.services &&
          stylist.services.map((service) => (
            <div key={service.serviceId}>
              <div>
                <Service
                  isSelected={selectedServices.find(
                    (e) => e.serviceId === service.serviceId
                  )}
                  serviceId={service.serviceId}
                  serviceName={service.service}
                  durationMinutes={service.durationMinutes}
                  price={service.price}
                  handleAddService={() => handleAddService(service)}
                ></Service>
              </div>
            </div>
          ))}
      </div>
      <div>
        <div className="full-calendar">
          <FullCalendar
            allDaySlot={false}
            plugins={[timeGridPlugin, interactionPlugin]}
            initialView="timeGridWeek"
            editable
            dateClick={(e) => selectTimeSlot(e)}
            events={
              appointment
                ? [...totalUnavailableBookingTime, appointment]
                : totalUnavailableBookingTime
            }
            businessHours={schedule.map((s) => ({
              daysOfWeek: [s.weekDayId - 1],
              startTime: s.startTime,
              endTime: s.endTime,
            }))}
          />
        </div>
      </div>
    </div>
  );
};

export default Appointment;
