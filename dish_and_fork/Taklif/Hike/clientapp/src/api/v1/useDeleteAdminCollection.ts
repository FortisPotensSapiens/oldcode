import { useMutation, UseMutationResult } from 'react-query';

import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseDeleteAdminCollection = () => UseMutationResult<string, unknown, { id: string }>;

const useDeleteAdminCollection: UseDeleteAdminCollection = () => {
  const ax = useAxios();

  return useMutation((data) => extractData(ax.delete(`/collections/admin/${data.id}`)));
};

export { useDeleteAdminCollection };
export type { UseDeleteAdminCollection };
