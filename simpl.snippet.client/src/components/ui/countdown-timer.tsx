import React, { useState, useEffect } from 'react';

interface CountdownTimerProps {
  expirationDate: Date;
}

const CountdownTimer: React.FC<CountdownTimerProps> = ({ expirationDate }) => {
  const calculateTimeLeft = () => {
    const difference = expirationDate.getTime() - new Date().getTime();
    let timeLeft: {
      hours?: number;
      minutes?: number;
      seconds?: number;
    } = {};

    if (difference > 0) {
      timeLeft = {
        hours: Math.floor(difference / (1000 * 60 * 60)),
        minutes: Math.floor((difference % (1000 * 60 * 60)) / (1000 * 60)),
        seconds: Math.floor((difference % (1000 * 60)) / 1000),
      };
    }

    return timeLeft;
  };

  const [timeLeft, setTimeLeft] = useState(calculateTimeLeft());

  useEffect(() => {
    const timer = setInterval(() => {
      setTimeLeft(calculateTimeLeft());
    }, 1000);

    return () => clearInterval(timer);
  }, []);

  const formatTime = (time: number | undefined) => {
    return time !== undefined && time < 10 ? `0${time}` : time;
  };

  return (
    <div>
      {timeLeft.hours !== undefined || timeLeft.minutes !== undefined || timeLeft.seconds !== undefined ? (
        <span>
          {formatTime(timeLeft.hours)}:{formatTime(timeLeft.minutes)}:{formatTime(timeLeft.seconds)}
        </span>
      ) : (
        <span>Время истекло!</span>
      )}
    </div>
  );
};

export default CountdownTimer;
