const plural = (a: number): 0 | 1 | 2 => {
  if (a % 10 === 1 && a % 100 !== 11) {
    return 0;
  }

  if (a % 10 >= 2 && a % 10 <= 4 && (a % 100 < 10 || a % 100 >= 20)) {
    return 1;
  }

  return 2;
};

const pluralize = (i: number, str0: string, str1: string, str2: string, str3: string): string => {
  if (i === 0) return str0;

  switch (plural(i)) {
    case 0:
      return str1;

    case 1:
      return str2;

    default:
      return str3;
  }
};

export { pluralize };
