import { useMutation, UseMutationResult } from 'react-query';

import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePutAdminExternalId = () => UseMutationResult<
  unknown,
  unknown,
  {
    id: string;
    externalId: string;
  },
  unknown
>;

const usePutAdminExternalId: UsePutAdminExternalId = () => {
  const { put } = useAxios();

  return useMutation((data) => extractData(put(`admin/partners/${data.id}/set-external-id/${data.externalId}`)));
};

export { usePutAdminExternalId };
export type { UsePutAdminExternalId };
