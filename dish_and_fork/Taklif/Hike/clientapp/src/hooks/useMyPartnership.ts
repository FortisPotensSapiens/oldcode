import { useGetMyPartnership } from '~/api';
import { PartnerReadModel } from '~/types';

type UseMyPartnership = (refetchOnMount?: boolean) => PartnerReadModel | undefined;

const useMyPartnership: UseMyPartnership = (refetchOnMount) => {
  const { data } = useGetMyPartnership(refetchOnMount);

  return data;
};

export { useMyPartnership };
export type { UseMyPartnership };
