import { UseQueryResult } from 'react-query';

import { useGetPartners } from '~/api';
import { useConfig } from '~/contexts';
import { PartnerReadModel } from '~/types';

type UsePartners = () => UseQueryResult<PartnerReadModel[]>;

const usePartners: UsePartners = () => {
  const { api } = useConfig();

  return useGetPartners(api.keys.getPartners);
};

export { usePartners };
