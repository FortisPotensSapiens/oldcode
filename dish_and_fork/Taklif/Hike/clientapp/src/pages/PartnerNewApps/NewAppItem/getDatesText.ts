import dayjs from 'dayjs';
import timezone from 'dayjs/plugin/timezone';
import utc from 'dayjs/plugin/utc';

dayjs.extend(utc);
dayjs.extend(timezone);

type GetDatesText = (from?: string | null, to?: string | null) => string | undefined;

const getDatesText: GetDatesText = (from, to) => {
  if (!from && !to) {
    return undefined;
  }

  const fromText = from ? dayjs(from).local().format('L') : '';
  const toText = to ? dayjs(to).local().format('L') : '';

  if (fromText === toText) {
    return fromText;
  }

  const result: string[] = [];

  if (fromText) {
    result.push(`c ${fromText}`);
  }

  if (toText) {
    result.push(`по ${toText}`);
  }

  return result.join(' ');
};

export { getDatesText };
export type { GetDatesText };
