import { useMutation, UseMutationResult } from 'react-query';

import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePostAdminUnblockPartner = () => UseMutationResult<string, unknown, { sellerId: string }, unknown>;

const usePostAdminUnblockPartner: UsePostAdminUnblockPartner = () => {
  const { post } = useAxios();

  return useMutation((data) => extractData(post(`/admin/partners/${data.sellerId}/unblock`)));
};

export { usePostAdminUnblockPartner };
export type { UsePostAdminUnblockPartner };
