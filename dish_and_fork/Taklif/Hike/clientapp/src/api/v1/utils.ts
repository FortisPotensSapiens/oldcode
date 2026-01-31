import qs from 'query-string';

export const buildTimestampString = (enabled: boolean) => {
  const props: Record<string, string> = {};

  if (enabled) {
    props.timestamp = String(Date.now());
  }

  return qs.stringify(props);
};
