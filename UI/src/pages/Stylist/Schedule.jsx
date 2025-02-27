import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import { DateTimePicker } from "@mui/x-date-pickers/DateTimePicker";
import { DemoContainer } from "@mui/x-date-pickers/internals/demo";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Select } from "../../components/widgets/Select";
import TextInput from "../../components/widgets/TextInput";
import { roleTypes } from "../../constants";
import useScheduleService from "../../services/ScheduleService";
import useStylistService from "../../services/StylistService";
import useUnavailableTimeServce from "../../services/UnavailableTimeServce";
import userSession from "../../utils/userSession";

const Schedule = () => {
  const [weekdaysList, setWeekdaysList] = useState([]);
  const [startTime, setStartTime] = useState([]);
  const [endTime, setEndTime] = useState([]);
  const [schedule, setSchedule] = useState(undefined);
  const [allHours, setAllHours] = useState(undefined);
  const [buttonIsPressed, setButtonIsPressed] = useState(false);
  const [scheduleIsLoading, setScheduleIsLoading] = useState(true);
  const [weekdaysAreLoading, setWeekdaysAreLoading] = useState(true);
  const [unavailableTimeButton, setUnavailableTimeButton] = useState(false);
  const [unavailableTimeStartTime, setUnavailableTimeStartTime] =
    useState(undefined);
  const [unavailableTimeEndTime, setUnavailableTimeEndTime] =
    useState(undefined);
  const [unavailableTimeReason, setUnavailableTimeReason] = useState("");
  const {
    getWeekdays,
    getScheduleByStylistId,
    createOrUpdateSchedule,
    deleteSchedule,
  } = useScheduleService();
  const { createUnavailableTime } = useUnavailableTimeServce();
  const user = userSession.user();
  const navigate = useNavigate();

  const { getStylistIdByUserId } = useStylistService();

  const fetchWeekdays = async () => {
    try {
      let weekdays = await getWeekdays();
      setWeekdaysList(weekdays);
      setWeekdaysAreLoading(false);
    } catch (e) {
      setWeekdaysAreLoading(false);
    }
  };

  const fetchSchedule = async () => {
    setButtonIsPressed(false);
    try {
      const stylistId = await getStylistIdByUserId(user.id);
      let scheduleFromDb = await getScheduleByStylistId(stylistId);

      let schedule = scheduleFromDb.map((scheduleObj) => {
        return { ...scheduleObj, isFreeDay: false };
      });
      setSchedule(schedule);

      const weekDays = [1, 2, 3, 4, 5, 6, 7];

      weekDays.forEach((weekDay) => {
        if (schedule.filter((s) => s.weekDayId == weekDay).length == 0) {
          schedule.push({
            weekDayId: weekDay,
            startTime: "00:00:00",
            endTime: "00:00:00",
            isFreeDay: true,
          });
        }
      });

      schedule = schedule.sort((s, t) => s.weekDayId - t.weekDayId);

      setSchedule(schedule);
      setScheduleIsLoading(false);
    } catch (e) {
      console.log(e);
      setScheduleIsLoading(false);
    }
  };

  const fetchAllHours = () => {
    let allHours = [];
    Array.from(Array(24).keys()).map((hour) => {
      if (hour < 10) {
        allHours.push({ id: `0${hour}:00:00`, name: `0${hour}:00:00` });
      } else {
        allHours.push({ id: `${hour}:00:00`, name: `${hour}:00:00` });
      }
    });
    setAllHours(allHours);
  };

  const handleSetFreeDay = async (weekDayId) => {
    const stylistId = await getStylistIdByUserId(user.id);
    await deleteSchedule(stylistId, weekDayId);
    setButtonIsPressed(true);
  };

  const handleSubmit = async (weekDayId, e) => {
    let localStartTime = startTime.filter((st) => st.id == weekDayId - 1)[0]
      .name;
    let localEndTime = endTime.filter((et) => et.id == weekDayId - 1)[0].name;
    const stylistId = await getStylistIdByUserId(user.id);
    const scheduleObject = {
      weekDayId: weekDayId,
      startTime: localStartTime,
      endTime: localEndTime,
    };
    await createOrUpdateSchedule(stylistId, scheduleObject);
    setButtonIsPressed(true);
  };

  const handleUnavailableTimeSubmit = async () => {
    const addHours = (n) => n * 60 * 60 * 1000;
    const stylistId = await getStylistIdByUserId(user.id);
    await createUnavailableTime(stylistId, {
      startDate: new Date(unavailableTimeStartTime.$d.getTime() + addHours(3)),
      endDate: new Date(unavailableTimeEndTime.$d.getTime() + addHours(3)),
      reason: unavailableTimeReason,
      stylistId: stylistId,
    });
  };

  useEffect(() => {
    if (user == null || user.roleId != roleTypes.stylistId) {
      navigate("/login");
    } else {
      fetchSchedule();
      fetchWeekdays();
      fetchAllHours();
    }
  }, []);

  useEffect(() => {
    fetchSchedule();
  }, [buttonIsPressed]);

  return (
    <div>
      {user && user.roleId == roleTypes.stylistId && (
        <div>
          <h1>Schedule update</h1>
          <div style={{ display: "flex" }}>
            {!weekdaysAreLoading &&
              !scheduleIsLoading &&
              allHours &&
              weekdaysList.map((weekday) => (
                <div key={weekday.id} className="time-pick-card">
                  <h2>{weekday.name}</h2>
                  <div>
                    <div>
                      <Select
                        disabled={false}
                        label={"Start time"}
                        sx={{ minWidth: 100 }}
                        options={allHours}
                        value={startTime[weekday.id - 1]}
                        onChange={(e) =>
                          setStartTime([
                            ...startTime,
                            { id: weekday.id - 1, name: e.target.value },
                          ])
                        }
                      ></Select>
                    </div>
                    <div>
                      <Select
                        disabled={false}
                        sx={{ minWidth: 100 }}
                        label={"End time"}
                        options={allHours}
                        value={endTime[weekday.id - 1]}
                        onChange={(e) =>
                          setEndTime([
                            ...endTime,
                            { id: weekday.id - 1, name: e.target.value },
                          ])
                        }
                      ></Select>
                    </div>
                    <div className="schedule-buttons">
                      <div className="schedule-buttons-set-free-day">
                        {!schedule[weekday.id - 1].isFreeDay && (
                          <button onClick={() => handleSetFreeDay(weekday.id)}>
                            Set free day
                          </button>
                        )}
                      </div>

                      <div className="schedule-buttons-set-submit">
                        <button onClick={(e) => handleSubmit(weekday.id, e)}>
                          Submit
                        </button>
                      </div>
                    </div>
                  </div>
                </div>
              ))}
          </div>
          <button
            onClick={() => setUnavailableTimeButton(!unavailableTimeButton)}
          >
            Add unavailable time period
          </button>
          {unavailableTimeButton && (
            <div>
              <LocalizationProvider dateAdapter={AdapterDayjs}>
                <DemoContainer
                  components={["DateTimePicker", "DateTimePicker"]}
                >
                  <DateTimePicker
                    label="Start time"
                    value={unavailableTimeStartTime}
                    onChange={(val) => setUnavailableTimeStartTime(val)}
                  />
                  <DateTimePicker
                    label="End time"
                    value={unavailableTimeEndTime}
                    onChange={(val) => setUnavailableTimeEndTime(val)}
                  />
                </DemoContainer>
              </LocalizationProvider>

              <TextInput
                style={{ marginTop: 20, marginBottom: 10 }}
                label="Reason(optional)"
                value={unavailableTimeReason}
                onChange={(e) => setUnavailableTimeReason(e.target.value)}
              ></TextInput>

              <button
                style={{ marginTop: 30 }}
                onClick={handleUnavailableTimeSubmit}
              >
                Submit
              </button>
            </div>
          )}
        </div>
      )}
    </div>
  );
};

export default Schedule;
