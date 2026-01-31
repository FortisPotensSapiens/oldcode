type GetSumText = (currency: string, from?: number | null, to?: number | null) => string | undefined;

const getSumText: GetSumText = (currency, from, to) => {
  if (!from && !to) {
    return undefined;
  }

  if (from && from === to) {
    return `${from} ${currency}`;
  }

  const result: string[] = [];

  if (from) {
    result.push(`от ${from} ${currency}`);
  }

  if (to) {
    result.push(`до ${to} ${currency}`);
  }

  return result.join(' ');
};

export { getSumText };
export type { GetSumText };
