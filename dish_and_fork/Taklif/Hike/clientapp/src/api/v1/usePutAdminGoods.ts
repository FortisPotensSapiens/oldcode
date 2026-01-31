import { useMutation, UseMutationResult } from 'react-query';

import { AdminMerchandiseUpdateModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePutAdminGoods = () => UseMutationResult<string, unknown, AdminMerchandiseUpdateModel, unknown>;

const usePutAdminGoods: UsePutAdminGoods = () => {
  const { put } = useAxios();

  return useMutation((data) => extractData(put('/admin/goods', data)));
};

export { usePutAdminGoods };
export type { UsePutAdminGoods };
