import React, { PropTypes as RPT, PureComponent as Component } from 'react';
import { connect } from 'react-redux';
import { fetchContours } from '../../common/api/actions';
import { FileUpload } from 'redux-file-upload';
import { LineChart } from 'react-d3-components';

@connect(state => ({
  contours: state.api.getIn(['contours', 'data']),
  contoursError: state.api.getIn(['contours', 'error']),
  contoursPending: state.api.getIn(['contours', 'pending']),
  uploadPending: state.app.get('uploadPending'),
}), { fetchContours })
export default class Page extends Component {

  static propTypes = {
    contours: RPT.array,
    contoursError: RPT.bool,
    contoursPending: RPT.bool,
    fetchContours: RPT.func.isRequired,
    uploadPending: RPT.bool
  }

  componentDidMount() {
    const { fetchContours } = this.props;
    fetchContours();
  }

  renderCharts() {
    const { contours } = this.props;
    if (!contours) return null;

    const data = contours.reduce((prev, x, idx) =>
      [...prev, {
        label: `chart${idx}`,
        values: Object.keys(x.timeline).reduce((prev2, key) =>
          [...prev2, { x: key, y: x.timeline[key] }]
        , [])
      }]
    , []);

    return (
      <LineChart
        data={data}
        width={2000}
        height={400}
      />
    );
  }

  render() {
    const { contoursError, contoursPending, uploadPending } = this.props;

    return (
      <div>
        <div>
          <FileUpload
            allowedFileTypes={['jpg', 'jpeg']}
            data={{ type: 'picture' }}
            dropzoneId="fileUpload"
            multiple
            url="/webapi/contours"
          >
            <button>
              Klikněte nebo přetáhněte JPG soubory
            </button>
          </FileUpload>
        </div>
        {contoursError && <p>Došlo k chybě. Určitě běží API?</p>}
        {uploadPending && <p>Odesílám fotky...</p>}
        {contoursPending && <p>Stahuji data a zpracovávám grafy...</p>}
        {this.renderCharts()}
      </div>
    );
  }
}
