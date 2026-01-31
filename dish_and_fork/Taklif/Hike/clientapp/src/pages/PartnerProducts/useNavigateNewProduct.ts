import { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';

import { getPartnerNewProductPath } from '~/routing';

type UseNavigateNewProduct = () => () => void;

const useNavigateNewProduct: UseNavigateNewProduct = () => {
  const navigate = useNavigate();

  return useCallback(() => {
    navigate(getPartnerNewProductPath());
  }, [navigate]);
};

export { useNavigateNewProduct };
export type { UseNavigateNewProduct };
