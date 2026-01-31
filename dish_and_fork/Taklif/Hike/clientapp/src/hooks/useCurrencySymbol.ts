import { getCurrencySymbol } from '~/utils';

import { useCurrency } from './useCurrency';

type UseCurrencySymbol = () => string;

const useCurrencySymbol: UseCurrencySymbol = () => getCurrencySymbol(useCurrency());

export { useCurrencySymbol };
export type { UseCurrencySymbol };
