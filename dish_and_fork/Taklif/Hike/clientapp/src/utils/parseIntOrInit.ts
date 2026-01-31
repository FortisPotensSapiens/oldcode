type ParseIntOrInit = (value: string | null | undefined, defaultValue: number) => number;

const parseIntOrInit: ParseIntOrInit = (value, defaultValue) => {
  if (!value) {
    return defaultValue;
  }

  const result = parseInt(value, 10);

  return isNaN(result) ? defaultValue : result;
};

export { parseIntOrInit };
export type { ParseIntOrInit };
