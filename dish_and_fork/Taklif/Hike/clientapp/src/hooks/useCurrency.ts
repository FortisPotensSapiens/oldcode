import { useGetConfig } from '~/api';
import { CurrencyType } from '~/types';

type UseCurrency = () => CurrencyType | undefined;

const useCurrency: UseCurrency = () => {
  const { data } = useGetConfig();

  return data?.currency;
};

export { useCurrency };
export type { UseCurrency };
