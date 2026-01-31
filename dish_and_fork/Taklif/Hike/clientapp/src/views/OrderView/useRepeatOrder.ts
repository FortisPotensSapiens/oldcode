import { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';

import { generateRepeatOrderPath } from '~/routing';

type UseRepeatOrder = (orderId?: string) => () => void;

const useRepeatOrder: UseRepeatOrder = (orderId) => {
  const navigate = useNavigate();

  return useCallback(() => navigate(generateRepeatOrderPath({ orderId })), [navigate, orderId]);
};

export { useRepeatOrder };
