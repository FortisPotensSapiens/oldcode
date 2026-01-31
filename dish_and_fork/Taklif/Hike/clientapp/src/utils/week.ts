const WEEK_DAY_RUS: Record<string, string> = {
  Monday: 'Понедельник',
  Tuesday: 'Вторник',
  Wednesday: 'Среда',
  Thursday: 'Четверг',
  Friday: 'Пятница',
  Saturday: 'Суббота',
  Sunday: 'Воскресенье',
};

export const getWeekDayLocaled = (engMonth: string, lang = 'ru') => {
  if (lang === 'ru') {
    return WEEK_DAY_RUS[engMonth] ?? '';
  }

  return '';
};
