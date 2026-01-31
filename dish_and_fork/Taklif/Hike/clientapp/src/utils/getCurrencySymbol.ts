import { CurrencyType } from '~/types';

const map: Record<CurrencyType, string> & Record<string, string> = { Rub: '₽' };

type GetCurrencySymbol = (currency?: string) => string;

const getCurrencySymbol: GetCurrencySymbol = (currency) => {
  const result = currency ? map[currency] : undefined;

  return result ?? '';
};

export { getCurrencySymbol };
