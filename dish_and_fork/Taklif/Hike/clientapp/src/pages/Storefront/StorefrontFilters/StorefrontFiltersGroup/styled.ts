import styled from '@emotion/styled';
import { Checkbox } from 'antd';

export const StyledCheckbox = styled(Checkbox)`
  margin-inline-start: 10px !important;
  padding-top: 5px;

  .ant-checkbox-inner {
    width: 20px;
    height: 20px;

    &::after {
      width: 8px;
      height: 12px;
    }
  }
`;

export const StyledGroup = styled(Checkbox.Group)`
  display: block;
`;
