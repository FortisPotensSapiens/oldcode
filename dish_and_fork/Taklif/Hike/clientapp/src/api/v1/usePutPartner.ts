import { useMutation, UseMutationResult } from 'react-query';

import { PartnerUpdateModel, UseQueryError } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePutPartner = () => UseMutationResult<string, UseQueryError, PartnerUpdateModel, unknown>;

const usePutPartner: UsePutPartner = () => {
  const { put } = useAxios();

  return useMutation((data) => extractData(put(`/seller/partners`, data)));
};

export { usePutPartner };
export type { UsePutPartner };
