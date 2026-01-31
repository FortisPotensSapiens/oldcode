import { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';

type UseNavigateOrder = (to: string) => () => void;

const useNavigateOrder: UseNavigateOrder = (to) => {
  const navigate = useNavigate();

  return useCallback(() => navigate(to), [navigate, to]);
};

export { useNavigateOrder };
