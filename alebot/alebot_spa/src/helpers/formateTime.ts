export const formatTime = (time: number): string => {
  const pad = (val: number) => (val < 10 ? `0${val}` : val);
  const minutes = Math.floor((time % 3600) / 60);
  const secs = time % 60;
  return `${pad(minutes)}:${pad(secs)}`;
};
