// ============================================================
// OfficeApp — Site JavaScript
// ============================================================

(function () {
    'use strict';

    // ======================== SIDEBAR TOGGLE ========================
    const sidebar = document.querySelector('.sidebar');
    const overlay = document.querySelector('.sidebar-overlay');
    const toggleBtns = document.querySelectorAll('.topbar-toggle, .sidebar-close');

    function toggleSidebar() {
        if (sidebar) {
            sidebar.classList.toggle('open');
            if (overlay) overlay.classList.toggle('active');
        }
    }

    function closeSidebar() {
        if (sidebar) {
            sidebar.classList.remove('open');
            if (overlay) overlay.classList.remove('active');
        }
    }

    toggleBtns.forEach(btn => btn.addEventListener('click', toggleSidebar));
    if (overlay) overlay.addEventListener('click', closeSidebar);

    // Close sidebar on resize to desktop
    window.addEventListener('resize', function () {
        if (window.innerWidth >= 992) closeSidebar();
    });

    // ======================== LOADING OVERLAY ========================
    const loadingOverlay = document.getElementById('loadingOverlay');

    function showLoading() {
        if (loadingOverlay) loadingOverlay.classList.add('active');
    }

    function hideLoading() {
        if (loadingOverlay) loadingOverlay.classList.remove('active');
    }

    // Attach to all forms (show loading on submit)
    document.querySelectorAll('form').forEach(function (form) {
        form.addEventListener('submit', function () {
            // Small delay to allow validation to run
            setTimeout(function () {
                const hasErrors = form.querySelector('.input-validation-error');
                if (!hasErrors) showLoading();
            }, 50);
        });
    });

    // ======================== AUTO-DISMISS ALERTS ========================
    document.querySelectorAll('.alert-float').forEach(function (alert) {
        setTimeout(function () {
            alert.style.transition = 'all 0.4s ease';
            alert.style.transform = 'translateX(120%)';
            alert.style.opacity = '0';
            setTimeout(function () { alert.remove(); }, 400);
        }, 4500);
    });

    // ======================== DELETE MODAL HANDLER ========================
    const deleteModal = document.getElementById('deleteConfirmModal');
    if (deleteModal) {
        deleteModal.addEventListener('show.bs.modal', function (event) {
            const trigger = event.relatedTarget;
            if (trigger) {
                const deleteUrl = trigger.getAttribute('data-delete-url');
                const itemName = trigger.getAttribute('data-item-name') || 'this item';
                const form = deleteModal.querySelector('#deleteForm');
                const nameSpan = deleteModal.querySelector('#deleteItemName');
                if (form) form.setAttribute('action', deleteUrl);
                if (nameSpan) nameSpan.textContent = itemName;
            }
        });
    }

    // ======================== TABLE SEARCH / FILTER ========================
    document.querySelectorAll('[data-table-search]').forEach(function (input) {
        const tableId = input.getAttribute('data-table-search');
        const table = document.getElementById(tableId);

        if (table) {
            input.addEventListener('input', function () {
                const filter = this.value.toLowerCase().trim();
                const rows = table.querySelectorAll('tbody tr');

                rows.forEach(function (row) {
                    if (row.classList.contains('table-empty-row')) {
                        return;
                    }
                    const text = row.textContent.toLowerCase();
                    row.style.display = text.includes(filter) ? '' : 'none';
                });

                // Update visible count
                updatePaginationInfo(tableId);
            });
        }
    });

    // ======================== CLIENT-SIDE PAGINATION ========================
    const ROWS_PER_PAGE = 10;

    function initPagination(tableId) {
        const table = document.getElementById(tableId);
        if (!table) return;

        const tbody = table.querySelector('tbody');
        if (!tbody) return;

        const rows = Array.from(tbody.querySelectorAll('tr:not(.table-empty-row)'));
        if (rows.length <= ROWS_PER_PAGE) {
            // Hide pagination if not needed
            const paginationContainer = document.querySelector('[data-pagination-for="' + tableId + '"]');
            if (paginationContainer) paginationContainer.style.display = 'none';
            return;
        }

        let currentPage = 1;
        const totalPages = Math.ceil(rows.length / ROWS_PER_PAGE);

        function showPage(page) {
            currentPage = page;
            rows.forEach(function (row, index) {
                if (row.style.display === 'none') return; // skip filtered rows
                const start = (page - 1) * ROWS_PER_PAGE;
                const end = start + ROWS_PER_PAGE;
                row.style.display = (index >= start && index < end) ? '' : 'none';
            });
            renderPaginationButtons(tableId, currentPage, totalPages);
            updatePaginationInfo(tableId);
        }

        showPage(1);
    }

    function renderPaginationButtons(tableId, currentPage, totalPages) {
        const container = document.querySelector('[data-pagination-for="' + tableId + '"] .pagination');
        if (!container) return;

        let html = '';
        html += '<li class="page-item ' + (currentPage === 1 ? 'disabled' : '') + '">';
        html += '<a class="page-link" href="#" data-page="' + (currentPage - 1) + '"><i class="bi bi-chevron-left"></i></a></li>';

        for (let i = 1; i <= totalPages; i++) {
            html += '<li class="page-item ' + (i === currentPage ? 'active' : '') + '">';
            html += '<a class="page-link" href="#" data-page="' + i + '">' + i + '</a></li>';
        }

        html += '<li class="page-item ' + (currentPage === totalPages ? 'disabled' : '') + '">';
        html += '<a class="page-link" href="#" data-page="' + (currentPage + 1) + '"><i class="bi bi-chevron-right"></i></a></li>';

        container.innerHTML = html;

        container.querySelectorAll('.page-link').forEach(function (link) {
            link.addEventListener('click', function (e) {
                e.preventDefault();
                const page = parseInt(this.getAttribute('data-page'));
                if (page >= 1 && page <= totalPages) {
                    // Re-show all non-filtered rows first
                    const table = document.getElementById(tableId);
                    const rows = Array.from(table.querySelectorAll('tbody tr:not(.table-empty-row)'));
                    const start = (page - 1) * ROWS_PER_PAGE;
                    const end = start + ROWS_PER_PAGE;
                    let visibleIndex = 0;
                    rows.forEach(function (row) {
                        // Only paginate visible (not search-hidden) rows
                        row.style.display = (visibleIndex >= start && visibleIndex < end) ? '' : 'none';
                        visibleIndex++;
                    });
                    renderPaginationButtons(tableId, page, totalPages);
                    updatePaginationInfo(tableId);
                }
            });
        });
    }

    function updatePaginationInfo(tableId) {
        const infoEl = document.querySelector('[data-pagination-info="' + tableId + '"]');
        if (!infoEl) return;
        const table = document.getElementById(tableId);
        if (!table) return;
        const allRows = table.querySelectorAll('tbody tr:not(.table-empty-row)');
        const visibleRows = Array.from(allRows).filter(r => r.style.display !== 'none');
        infoEl.textContent = 'Showing ' + visibleRows.length + ' of ' + allRows.length + ' entries';
    }

    // Auto-init pagination for tables with [data-paginate]
    document.querySelectorAll('table[data-paginate]').forEach(function (table) {
        initPagination(table.id);
    });

    // ======================== TABLE SORTING ========================
    document.querySelectorAll('.table thead th.sortable').forEach(function (th) {
        th.addEventListener('click', function () {
            const table = th.closest('table');
            const tbody = table.querySelector('tbody');
            const rows = Array.from(tbody.querySelectorAll('tr:not(.table-empty-row)'));
            const colIndex = Array.from(th.parentNode.children).indexOf(th);
            const isAsc = th.classList.contains('sort-asc');

            // Remove sort classes from all headers
            table.querySelectorAll('thead th').forEach(h => {
                h.classList.remove('sort-asc', 'sort-desc');
            });

            // Toggle sort direction
            th.classList.add(isAsc ? 'sort-desc' : 'sort-asc');
            const direction = isAsc ? -1 : 1;

            rows.sort(function (a, b) {
                const aText = a.children[colIndex]?.textContent.trim().toLowerCase() || '';
                const bText = b.children[colIndex]?.textContent.trim().toLowerCase() || '';
                const aNum = parseFloat(aText);
                const bNum = parseFloat(bText);

                if (!isNaN(aNum) && !isNaN(bNum)) {
                    return (aNum - bNum) * direction;
                }
                return aText.localeCompare(bText) * direction;
            });

            rows.forEach(row => tbody.appendChild(row));
        });
    });

    // ======================== EXPOSE GLOBALS ========================
    window.OfficeApp = {
        showLoading: showLoading,
        hideLoading: hideLoading,
        toggleSidebar: toggleSidebar,
        closeSidebar: closeSidebar
    };

})();
