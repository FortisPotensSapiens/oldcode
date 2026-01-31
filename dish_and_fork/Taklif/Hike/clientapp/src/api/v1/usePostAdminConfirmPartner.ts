import { useMutation, UseMutationOptions, UseMutationResult } from 'react-query';

import { UseQueryError } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePostAdminConfirmPartner = (
  options?: Omit<UseMutationOptions<number, UseQueryError, string>, 'mutationFn'>,
) => UseMutationResult<number, UseQueryError, string>;

const usePostAdminConfirmPartner: UsePostAdminConfirmPartner = (options) => {
  const { post } = useAxios();

  return useMutation((id) => extractData(post(`/admin/partners/confirm/${id}`)), options);
};

export { usePostAdminConfirmPartner };
export type { UsePostAdminConfirmPartner };
