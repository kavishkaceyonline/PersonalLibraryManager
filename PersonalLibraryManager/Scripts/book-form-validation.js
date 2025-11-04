using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalLibraryManager.Scripts
{
	public class book_form_validation
	{
        $(document).ready(function() {
            // Custom validation for DateCompleted based on Status
            var statusDropdown = $('#Status');
            var dateCompletedGroup = $('#DateCompleted').closest('.form-group');

            function toggleDateCompletedValidation() {
                var selectedStatus = statusDropdown.val();

                if (selectedStatus === '3') { // Completed = 3
                    dateCompletedGroup.show();
                } else {
                    dateCompletedGroup.show(); // Keep visible but not required
                    $('#DateCompleted').val(''); // Clear value
                }
            }

            // Run on page load
            toggleDateCompletedValidation();

            // Run when status changes
            statusDropdown.on('change', toggleDateCompletedValidation);

            // Form submission validation
            $('form').on('submit', function (e) {
                var selectedStatus = statusDropdown.val();
                var dateCompleted = $('#DateCompleted').val();

                if (selectedStatus === '3' && !dateCompleted) {
                    e.preventDefault();
                    alert('Please enter the date you completed this book.');
                    $('#DateCompleted').focus();
                    return false;
                }
            });
        });
	}
}