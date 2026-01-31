import { useMutation, UseMutationResult } from 'react-query';

import { PartnerCreateModel, UseQueryError } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePostPartner = () => UseMutationResult<string, UseQueryError, PartnerCreateModel, unknown>;

const usePostPartner: UsePostPartner = () => {
  const { post } = useAxios();

  return useMutation((data) => extractData(post(`/partners`, data)));
};

export { usePostPartner };
export type { UsePostPartner };
