import { useMutation, UseMutationResult } from 'react-query';

import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePostAdminBlockPartner = () => UseMutationResult<string, unknown, { sellerId: string }, unknown>;

const usePostAdminBlockPartner: UsePostAdminBlockPartner = () => {
  const { post } = useAxios();

  return useMutation((data) => extractData(post(`/admin/partners/${data.sellerId}/block`)));
};

export { usePostAdminBlockPartner };
export type { UsePostAdminBlockPartner };
