import styled from '@emotion/styled';
import React, { FC, useState } from 'react';

const Container = styled.div`
  position: relative;
  margin-bottom: 0px;

  & .ant-input {
    padding: 18px 12px 9px 15px;
  }

  & .ant-select .ant-select-selector {
    padding: 16px 10px 7px 11px;
  }

  & .ant-select-single:not(.ant-select-customize-input) .ant-select-selector {
    padding: 16px 10px 7px 11px;
    height: 48px;
  }

  & .ant-select-single .ant-select-selector .ant-select-selection-search {
    top: 16px;
  }
`;

const Required = styled.span`
  color: red;
  padding-left: 3px;
`;

const Label = styled.label<{ float: boolean | string | undefined; left: number }>`
  font-size: 1rem;
  font-weight: normal;
  position: absolute;
  pointer-events: none;
  top: 12px;
  transition: 0.2s ease all;
  opacity: 0.6;
  ${({ float }) => float && 'top: 4px; font-size: 0.7rem;'}
  ${({ left }) => `left: ${left}px;`}
`;
const FloatLabel: FC<{ label: string; value?: string; required?: boolean; left?: number; className?: string }> = (
  props,
) => {
  const [focus, setFocus] = useState(false);
  const { children, label, value, required, left = 15, className } = props;

  return (
    <Container onBlur={() => setFocus(false)} onFocus={() => setFocus(true)} className={className}>
      {children}
      <Label float={focus || (value && value.length !== 0)} left={left}>
        {label}
        {required ? <Required>*</Required> : undefined}
      </Label>
    </Container>
  );
};

export default FloatLabel;
