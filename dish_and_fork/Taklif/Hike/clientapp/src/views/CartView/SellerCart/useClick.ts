import { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';

import { generatePlaceOrderPath } from '~/routing';

type UseClick = (sellerId: string) => () => void;

const useClick: UseClick = (sellerId) => {
  const navigate = useNavigate();

  return useCallback(() => navigate(generatePlaceOrderPath({ sellerId })), [navigate, sellerId]);
};

export default useClick;
