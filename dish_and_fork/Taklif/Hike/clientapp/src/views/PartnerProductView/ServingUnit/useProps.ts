import { useCallback, useState } from 'react';

type UseProps = (initial?: boolean) => {
  focused: boolean;
  hovered: boolean;
  onBlur: () => void;
  onFocus: () => void;
  onMouseEnter: () => void;
  onMouseLeave: () => void;
};

const useProps: UseProps = () => {
  const [focused, setFocused] = useState(false);
  const [hovered, setHovered] = useState(false);

  const onMouseEnter = useCallback(() => setHovered(true), []);
  const onMouseLeave = useCallback(() => setHovered(false), []);
  const onFocus = useCallback(() => setFocused(true), []);

  const onBlur = useCallback(() => {
    setFocused(false);
    onMouseLeave();
  }, [onMouseLeave]);

  return { focused, hovered, onBlur, onFocus, onMouseEnter, onMouseLeave };
};

export { useProps };
export type { UseProps };
